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
using NomaNova.Ojeda.Services.Shared.Background.Interfaces;
using NomaNova.Ojeda.Services.Shared.FileStore;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments;

public class AssetAttachmentsService : IAssetAttachmentsService 
{
    private class ProcessedFile
    {
        public string DisplayFileName { get; init; }

        public string ContentType { get; init; }

        public long SizeInBytes { get; init; }

        public byte[] Data { get; init; }
    }

    private readonly FileStoreOptions _options;
    private readonly IJobService _jobService;
    private readonly IMapper _mapper;
    private readonly IAssetFileStore _assetFileStore;
    private readonly IRepository<Asset> _assetsRepository;
    private readonly IRepository<AssetAttachment> _assetAttachmentsRepository;
    private readonly IThumbnailGenerator _thumbnailGenerator;

    public AssetAttachmentsService(
        IOptions<FileStoreOptions> options,
        IJobService jobService,
        IMapper mapper,
        IAssetFileStore assetFileStore,
        IRepository<Asset> assetsRepository,
        IRepository<AssetAttachment> assetAttachmentsRepository,
        IThumbnailGenerator thumbnailGenerator)
    {
        _options = options.Value;
        _jobService = jobService;
        _mapper = mapper;
        _assetFileStore = assetFileStore;
        _assetsRepository = assetsRepository;
        _assetAttachmentsRepository = assetAttachmentsRepository;
        _thumbnailGenerator = thumbnailGenerator;
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

    public async Task<FileDownload> GetRawDownloadAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(id, cancellationToken);

        if (assetAttachment == null)
        {
            throw new NotFoundException();
        }

        var absolutePath = _assetFileStore.GetAttachmentAbsolutePath(assetAttachment.AssetId, assetAttachment.StorageFileName);
        
        return new FileDownload
        {
            ContentType = assetAttachment.ContentType,
            FileName = assetAttachment.DisplayFileName,
            AbsolutePath = absolutePath
        };
    }

    public async Task<FileDownload> GetThumbnailDownloadAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(id, cancellationToken);

        if (assetAttachment == null)
        {
            throw new NotFoundException();
        }
        
        var absolutePath = _assetFileStore.GetAttachmentThumbnailAbsolutePath(assetAttachment.AssetId, assetAttachment.StorageFileName);
        
        return new FileDownload
        {
            ContentType = _thumbnailGenerator.ThumbnailContentType,
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
        var storageFileName = await _assetFileStore.StoreAttachmentAsync(asset.Id, processedFile.Data, cancellationToken);

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
            await _assetFileStore.DeleteAttachmentAsync(asset.Id, oldStorageFileName, cancellationToken);
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

        // Trigger thumbnail generation
        _jobService.Fire(() => _thumbnailGenerator.GenerateForAssetAttachment(assetAttachment.Id, null));
        
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
        await _assetFileStore.DeleteAttachmentAsync(assetAttachment.AssetId, assetAttachment.StorageFileName, cancellationToken);

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
}