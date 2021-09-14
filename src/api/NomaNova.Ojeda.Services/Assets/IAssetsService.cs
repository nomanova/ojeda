using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Services.Assets
{
    public interface IAssetsService
    {
        Task<AssetDto> GetByAssetClassAsync(
            string assetClassId, CancellationToken cancellationToken = default);
        
        Task<AssetDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<AssetSummaryDto>> GetAsync(
            string searchQuery,
            string orderBy,
            bool orderAsc,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<AssetDto> CreateAsync(AssetDto assetDto, CancellationToken cancellationToken = default);

        Task<AssetDto> UpdateAsync(string id, AssetDto assetDto,
            CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}