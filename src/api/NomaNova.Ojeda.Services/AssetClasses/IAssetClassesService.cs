using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;

namespace NomaNova.Ojeda.Services.AssetClasses
{
    public interface IAssetClassesService
    {
        Task<AssetClassDto> GetAssetClassByIdAsync(
            string id, CancellationToken cancellationToken = default);
        
        Task<PaginatedListDto<AssetClassDto>> GetAssetClassesAsync(
            string searchQuery, 
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default);
        
        Task<AssetClassDto> CreateAssetClassAsync(
            AssetClassDto assetClassDto, CancellationToken cancellationToken = default);
        
        Task<AssetClassDto> UpdateAssetClassAsync(
            string id, AssetClassDto assetClassDto, CancellationToken cancellationToken = default);
        
        Task DeleteAssetClassAsync(string id, CancellationToken cancellationToken = default);
    }
}