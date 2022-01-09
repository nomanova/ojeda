using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;
using NomaNova.Ojeda.Services.Shared.FileStore.Interfaces;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments;

public class AssetFileStore : IAssetFileStore
{
    private const string AssetsDirectory = "assets";
    private const string ThumbnailPrefix = "thumbnail";

    private readonly IFileStore _fileStore;

    public AssetFileStore(IFileStore fileStore)
    {
        _fileStore = fileStore;
    }

    public string GetAttachmentAbsolutePath(string assetId, string fileName)
    {
        return _fileStore.ToAbsolutePath(GetPath(assetId, fileName));
    }

    public string GetAttachmentThumbnailAbsolutePath(string assetId, string fileName)
    {
        var thumbnailFileName = GetThumbnailFileName(fileName);
        return _fileStore.ToAbsolutePath(GetPath(assetId, thumbnailFileName));
    }

    public async Task<string> StoreAttachmentAsync(string assetId, byte[] data,
        CancellationToken cancellationToken = default)
    {
        var fileName = Path.GetRandomFileName();
        var filePath = GetPath(assetId, fileName);

        await _fileStore.WriteBytesAsync(filePath, data, cancellationToken);

        return fileName;
    }

    public async Task<string> StoreAttachmentThumbnailAsync(string assetId, string fileName, byte[] data,
        CancellationToken cancellationToken = default)
    {
        var thumbnailFileName = GetThumbnailFileName(fileName);
        var filePath = GetPath(assetId, thumbnailFileName);

        await _fileStore.WriteBytesAsync(filePath, data, cancellationToken);

        return thumbnailFileName;
    }

    public async Task<byte[]> LoadAttachmentAsync(string assetId, string fileName,
        CancellationToken cancellationToken = default)
    {
        var filePath = GetPath(assetId, fileName);

        return await _fileStore.ReadBytesAsync(filePath, cancellationToken);
    }

    public async Task DeleteAttachmentAsync(string assetId, string fileName,
        CancellationToken cancellationToken = default)
    {
        var filePath = GetPath(assetId, fileName);
        await _fileStore.DeleteAsync(filePath, cancellationToken);

        var thumbnailFilePath = GetPath(assetId, GetThumbnailFileName(fileName));
        await _fileStore.DeleteAsync(thumbnailFilePath, cancellationToken);
    }

    private static string GetPath(string assetId, string fileName)
    {
        return Path.Combine(AssetsDirectory, assetId, fileName);
    }

    private static string GetThumbnailFileName(string fileName)
    {
        return $"{ThumbnailPrefix}.{fileName}";
    }
}