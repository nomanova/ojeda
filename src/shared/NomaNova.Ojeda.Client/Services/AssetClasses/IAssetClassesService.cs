using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.AssetClasses
{
    public interface IAssetClassesService
    {
        Task<OjedaDataResult<AssetClassDto>> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<PaginatedListDto<AssetClassDto>>> GetAsync(
            string query = null,
            string orderBy = null, 
            bool orderAsc = true,
            int pageNumber = 1, 
            int pageSize = 10, 
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetClassDto>> CreateAsync(AssetClassDto assetClass,
            CancellationToken cancellationToken = default);
        
        Task<OjedaDataResult<AssetClassDto>> UpdateAsync(string id, AssetClassDto assetClass,
            CancellationToken cancellationToken = default);

        Task<OjedaResult> DeleteAsync(
            string id, CancellationToken cancellationToken = default);
    }
}