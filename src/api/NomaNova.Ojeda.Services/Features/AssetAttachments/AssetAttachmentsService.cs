using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Core.Domain.AssetAttachments;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Exceptions;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetAttachments;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Models;
using NomaNova.Ojeda.Services.Shared.FileStore;
using NomaNova.Ojeda.Services.Shared.FileStore.Interfaces;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments;

public class AssetAttachmentsService : IAssetAttachmentsService
{
    private const string AssetsDirectory = "assets";

    private class ProcessedFile
    {
        public string DisplayFileName { get; set; }

        public string ContentType { get; set; }

        public long SizeInBytes { get; set; }

        public byte[] Data { get; set; }
    }

    private readonly FileStoreOptions _options;
    private readonly IMapper _mapper;
    private readonly IFileStore _fileStore;
    private readonly IRepository<Asset> _assetsRepository;
    private readonly IRepository<AssetAttachment> _assetAttachmentsRepository;

    public AssetAttachmentsService(
        IOptions<FileStoreOptions> options,
        IMapper mapper,
        IFileStore fileStore,
        IRepository<Asset> assetsRepository,
        IRepository<AssetAttachment> assetAttachmentsRepository)
    {
        _options = options.Value;
        _mapper = mapper;
        _fileStore = fileStore;
        _assetsRepository = assetsRepository;
        _assetAttachmentsRepository = assetAttachmentsRepository;
    }

    public async Task<AssetAttachmentDto> GetByIdAsync(
        string id, CancellationToken cancellationToken = default)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(id, cancellationToken);

        if (assetAttachment == null)
        {
            throw new NotFoundException();
        }

        return _mapper.Map<AssetAttachmentDto>(assetAttachment);
    }

    public async Task<AssetAttachmentDownload> GetAsDownloadAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(id, cancellationToken);

        if (assetAttachment == null)
        {
            throw new NotFoundException();
        }

        var assetAttachmentPath = GetPath(assetAttachment.AssetId, assetAttachment.StorageFileName);
        var absolutePath = _fileStore.ToAbsolutePath(assetAttachmentPath);

        return new AssetAttachmentDownload
        {
            ContentType = assetAttachment.ContentType,
            FileName = assetAttachment.DisplayFileName,
            AbsolutePath = absolutePath
        };
    }

    public async Task<AssetAttachmentDto> CreateAsync(CreateAssetAttachmentDto createAssetAttachment,
        CancellationToken cancellationToken = default)
    {
        // Fetch asset
        var asset = await _assetsRepository.GetByIdAsync(createAssetAttachment.AssetId, cancellationToken);

        if (asset == null)
        {
            throw new NotFoundException();
        }

        // Process file
        var allowedExtensions = createAssetAttachment.IsPrimary
            ? FileTypeProvider.FileTypes.Where(_ => _.IsImage).Select(_ => _.Extension).ToList()
            : FileTypeProvider.FileTypes.Select(_ => _.Extension).ToList();

        var processedFile =
            await ProcessFormFileAsync(createAssetAttachment.File, _options.MaxSizeInBytes, allowedExtensions);

        // Store file on disk
        var storageFileName = Path.GetRandomFileName();
        var filePath = GetPath(asset.Id, storageFileName);

        await _fileStore.WriteBytesAsync(filePath, processedFile.Data, cancellationToken);

        // Store database entry
        AssetAttachment assetAttachment;

        var primaryAssetAttachment =
            (await _assetAttachmentsRepository.GetAllAsync(
                query => { return query.Where(_ => _.AssetId == asset.Id && _.IsPrimary); }, cancellationToken))
            .FirstOrDefault();

        if (createAssetAttachment.IsPrimary && primaryAssetAttachment != null)
        {
            var oldStorageFileName = primaryAssetAttachment.StorageFileName;

            // Update existing primary
            primaryAssetAttachment.DisplayFileName = processedFile.DisplayFileName;
            primaryAssetAttachment.StorageFileName = storageFileName;
            primaryAssetAttachment.ContentType = processedFile.ContentType;
            primaryAssetAttachment.SizeInBytes = processedFile.SizeInBytes;

            assetAttachment = await _assetAttachmentsRepository.UpdateAsync(primaryAssetAttachment, cancellationToken);

            // Delete existing file
            var deletableFilePath = GetPath(asset.Id, oldStorageFileName);
            await _fileStore.DeleteAsync(deletableFilePath, cancellationToken);
        }
        else
        {
            var newAssetAttachment = new AssetAttachment
            {
                Id = Guid.NewGuid().ToString(),
                AssetId = asset.Id,
                DisplayFileName = processedFile.DisplayFileName,
                StorageFileName = storageFileName,
                ContentType = processedFile.ContentType,
                SizeInBytes = processedFile.SizeInBytes,
                IsPrimary = createAssetAttachment.IsPrimary
            };

            assetAttachment = await _assetAttachmentsRepository.InsertAsync(newAssetAttachment, cancellationToken);
        }

        return _mapper.Map<AssetAttachmentDto>(assetAttachment);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(id, cancellationToken);

        if (assetAttachment == null)
        {
            throw new NotFoundException();
        }

        // Remove file
        var deletableFilePath = GetPath(assetAttachment.AssetId, assetAttachment.StorageFileName);
        await _fileStore.DeleteAsync(deletableFilePath, cancellationToken);

        await _assetAttachmentsRepository.DeleteAsync(assetAttachment, cancellationToken);
    }

    private static async Task<ProcessedFile> ProcessFormFileAsync(IFormFile formFile, long sizeLimitInBytes,
        ICollection<string> allowedExtensions)
    {
        // Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
        var fileName = formFile.FileName;
        var trustedFileName = WebUtility.HtmlEncode(fileName);

        var fileLength = formFile.Length;

        // Check the file length. This check doesn't catch files that only have a BOM as their content.
        if (fileLength == 0)
        {
            throw new ValidationException(nameof(CreateAssetAttachmentDto.File),
                $"File {trustedFileName} is empty.");
        }

        if (fileLength > sizeLimitInBytes)
        {
            var sizeLimitInMegaBytes = sizeLimitInBytes / 1_048_576;
            throw new ValidationException(nameof(CreateAssetAttachmentDto.File),
                $"File {trustedFileName} exceeds limit of {sizeLimitInMegaBytes:N1} MB.");
        }

        try
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            // Check the content length in case the file's only content was a BOM,
            // and the content is actually empty after removing the BOM.
            if (memoryStream.Length == 0)
            {
                throw new ValidationException(nameof(CreateAssetAttachmentDto.File),
                    $"File {trustedFileName} is empty.");
            }

            var fileType = DetermineFileType(fileName, memoryStream, allowedExtensions);

            if (fileType == null)
            {
                throw new ValidationException(nameof(CreateAssetAttachmentDto.File),
                    $"File {trustedFileName} file type is not permitted or file signature does not match extension.");
            }

            return new ProcessedFile
            {
                DisplayFileName = trustedFileName,
                SizeInBytes = fileLength,
                ContentType = fileType.ContentType,
                Data = memoryStream.ToArray()
            };
        }
        catch (Exception ex)
        {
            throw new ValidationException(nameof(CreateAssetAttachmentDto.File),
                $"File {trustedFileName} upload failed: {ex.Message}.");
        }
    }

    private static FileType DetermineFileType(string fileName, Stream data, ICollection<string> allowedExtensions)
    {
        if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
        {
            return null;
        }

        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
        {
            return null;
        }

        data.Position = 0;

        using var reader = new BinaryReader(data);

        var fileType = FileTypeProvider.ForExtension(ext);

        if (fileType == null)
        {
            return null;
        }

        var headerBytes = reader.ReadBytes(fileType.Signatures.Max(m => m.Length));
        return fileType.Signatures.Any(_ => headerBytes.Take(_.Length).SequenceEqual(_)) ? fileType : null;
    }

    private static string GetPath(string assetId, string fileName)
    {
        return Path.Combine(AssetsDirectory, assetId, fileName);
    }
}