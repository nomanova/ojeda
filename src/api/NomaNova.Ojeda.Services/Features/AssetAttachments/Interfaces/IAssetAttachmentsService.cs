using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.AssetAttachments;
using NomaNova.Ojeda.Services.Shared.FileStore;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;

public interface IAssetAttachmentsService
{
    Task<AssetAttachmentDto> GetByIdAsync(
        string id, CancellationToken cancellationToken = default);

    Task<FileDownload> GetRawDownloadAsync(string id,
        CancellationToken cancellationToken = default);

    Task<FileDownload> GetThumbnailDownloadAsync(string id,
        CancellationToken cancellationToken = default);
    
    Task<AssetAttachmentDto> CreateAsync(CreateAssetAttachmentDto createAssetAttachment,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}