using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Client.Services.Assets
{
    public interface IAssetsService
    {
        Task<OjedaDataResult<AssetDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<AssetDto>> GetByAssetClass(
            string assetClassId,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<AssetDto>> CreateAsync(
            AssetDto asset, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<AssetDto>> UpdateAsync(
            string id, AssetDto asset,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}