using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;

namespace NomaNova.Ojeda.Services.AssetClasses
{
    public interface IAssetClassesService
    {
        Task<AssetClassDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);
        
        Task<PaginatedListDto<AssetClassDto>> GetAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<AssetClassDto> CreateAsync(
            AssetClassDto assetClassDto, CancellationToken cancellationToken = default);
        
        Task<AssetClassDto> UpdateAsync(
            string id, AssetClassDto assetClassDto, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}