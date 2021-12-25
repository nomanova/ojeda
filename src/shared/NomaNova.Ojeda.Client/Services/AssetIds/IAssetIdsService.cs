using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.AssetIds;

namespace NomaNova.Ojeda.Client.Services.AssetIds;

public interface IAssetIdsService
{
    Task<OjedaDataResult<GenerateAssetIdDto>> GenerateAsync(string assetTypeId,
        CancellationToken cancellationToken = default);
}