using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.AssetTypes
{
    public interface IAssetTypesService
    {
        Task<AssetTypeDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);
        
        Task<PaginatedListDto<AssetTypeDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<AssetTypeDto> CreateAsync(
            AssetTypeDto assetTypeDto, CancellationToken cancellationToken = default);
        
        Task<AssetTypeDto> UpdateAsync(
            string id, AssetTypeDto assetTypeDto, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}