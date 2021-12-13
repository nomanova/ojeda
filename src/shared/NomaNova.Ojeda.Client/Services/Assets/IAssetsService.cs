using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.Assets
{
    public interface IAssetsService
    {
        Task<OjedaDataResult<AssetDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<AssetDto>> GetByAssetType(
            string assetTypeId,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<PaginatedListDto<AssetSummaryDto>>> GetAsync(
            string query = null,
            string orderBy = null,
            bool orderAsc = true,
            int pageNumber = Constants.DefaultPageNumber,
            int pageSize = Constants.DefaultPageSize,
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetDto>> CreateAsync(
            CreateAssetDto asset, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<AssetDto>> UpdateAsync(
            string id, UpdateAssetDto asset,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}