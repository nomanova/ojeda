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
            int pageNumber = 1, 
            int pageSize = 10, 
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetTypeDto>> CreateAsync(AssetTypeDto assetType,
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetTypeDto>> UpdateAsync(string id, AssetTypeDto assetType,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(
            string id, CancellationToken cancellationToken = default);
    }
}