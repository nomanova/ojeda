using System.Threading.Tasks;
using Hangfire.Server;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;

public interface IThumbnailGenerator
{
    string ThumbnailContentType { get; }

    Task GenerateForAssetAttachment(string assetAttachmentId, PerformContext ctx = null);
}