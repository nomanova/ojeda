using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using NomaNova.Ojeda.Core.Domain.AssetAttachments;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;
using NomaNova.Ojeda.Services.Shared.FileStore;
using NomaNova.Ojeda.Utils.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments;

public class ThumbnailGenerator : IThumbnailGenerator
{
    private const int ThumbnailSize = 512;

    private readonly ILogger<ThumbnailGenerator> _logger;
    private readonly IAssetFileStore _assetFileStore;
    private readonly IRepository<AssetAttachment> _assetAttachmentsRepository;

    public ThumbnailGenerator(
        ILogger<ThumbnailGenerator> logger,
        IAssetFileStore assetFileStore,
        IRepository<AssetAttachment> assetAttachmentsRepository)
    {
        _logger = logger;
        _assetFileStore = assetFileStore;
        _assetAttachmentsRepository = assetAttachmentsRepository;
    }

    public string ThumbnailContentType => FileTypeProvider.ContentTypeImageJpeg;

    [AutomaticRetry(Attempts = 0)]
    [DisplayName("Thumbnail for Asset Attachment {0}")]
    public async Task GenerateForAssetAttachment(string assetAttachmentId, PerformContext ctx = null)
    {
        var assetAttachment = await _assetAttachmentsRepository.GetByIdAsync(assetAttachmentId, CancellationToken.None);

        if (assetAttachment == null)
        {
            _logger.LogInformation("Aborting thumbnail generation, unknown attachment {Id}", assetAttachmentId);
            return;
        }

        var assetId = assetAttachment.AssetId;

        var data = await _assetFileStore.LoadAttachmentAsync(assetId, assetAttachment.StorageFileName,
            CancellationToken.None);

        if (!data.HasElements())
        {
            _logger.LogInformation("Aborting thumbnail generation, no data for {Id}", assetAttachmentId);
            return;
        }

        byte[] thumbnailData = null;

        switch (assetAttachment.ContentType)
        {
            case FileTypeProvider.ContentTypeImagePng:
            case FileTypeProvider.ContentTypeImageJpg:
            case FileTypeProvider.ContentTypeImageJpeg:
                thumbnailData = GenerateImageThumbnail(data);
                break;
            default:
                _logger.LogWarning("Unsupported content type {ContentType}", assetAttachment.ContentType);
                break;
        }

        if (!thumbnailData.HasElements())
        {
            return;
        }

        var thumbnailFileName = await _assetFileStore.StoreAttachmentThumbnailAsync(assetId,
            assetAttachment.StorageFileName, thumbnailData,
            CancellationToken.None);

        _logger.LogInformation("Generated thumbnail for {Id} at {FileName}", assetAttachmentId, thumbnailFileName);
    }

    private byte[] GenerateImageThumbnail(byte[] data)
    {
        try
        {
            using var image = Image.Load(data);

            var width = image.Width;
            var height = image.Height;

            if (width > height)
            {
                image.Mutate(x => x.Resize(ThumbnailSize, 0));
            }
            else
            {
                image.Mutate(x => x.Resize(0, ThumbnailSize));
            }

            using var stream = new MemoryStream();
            image.SaveAsJpeg(stream);

            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not generate image thumbnail: {Message}", ex.Message);
            return null;
        }
    }
}