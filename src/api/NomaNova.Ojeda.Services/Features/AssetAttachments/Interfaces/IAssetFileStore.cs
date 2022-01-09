using System.Threading;
using System.Threading.Tasks;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;

public interface IAssetFileStore
{
    string GetAttachmentAbsolutePath(string assetId, string fileName);

    string GetAttachmentThumbnailAbsolutePath(string assetId, string fileName);
    
    Task<string> StoreAttachmentAsync(string assetId, byte[] data,
        CancellationToken cancellationToken = default);

    Task<string> StoreAttachmentThumbnailAsync(string assetId, string fileName, byte[] data,
        CancellationToken cancellationToken = default);
    
    Task<byte[]> LoadAttachmentAsync(string assetId, string fileName,
        CancellationToken cancellationToken = default);

    Task DeleteAttachmentAsync(string assetId, string fileName,
        CancellationToken cancellationToken = default);
}