using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Models.Dtos.AssetIds;

namespace NomaNova.Ojeda.Services.Features.AssetIds.Interfaces;

public interface IAssetIdsService
{
    Task<GenerateAssetIdDto> GenerateAssetId(string assetTypeId, CancellationToken cancellationToken = default);
    
    Task<(string assetId, string fullAssetId)> GenerateAssetId(AssetIdType assetIdType, CancellationToken cancellationToken = default);
}