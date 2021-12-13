using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.AssetTypes
{
    public interface IAssetTypesService
    {
        Task<OjedaDataResult<AssetTypeDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<PaginatedListDto<AssetTypeDto>>> GetAsync(
            string query = null,
            string orderBy = null, 
            bool orderAsc = true,
            int pageNumber = Constants.DefaultPageNumber, 
            int pageSize = Constants.DefaultPageSize, 
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetTypeDto>> CreateAsync(CreateAssetTypeDto assetType,
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetTypeDto>> UpdateAsync(string id, UpdateAssetTypeDto assetType,
            CancellationToken cancellationToken = default);

        Task<OjedaDataResult<DryRunUpdateAssetTypeDto>> DryRunUpdateAsync(string id,
            UpdateAssetTypeDto assetType, CancellationToken cancellationToken = default);
        
        Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task<OjedaDataResult<DryRunDeleteAssetTypeDto>> DryRunDeleteAsync(string id,
            CancellationToken cancellationToken = default);
    }
}