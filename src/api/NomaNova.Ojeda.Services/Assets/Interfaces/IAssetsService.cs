using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Assets.Interfaces
{
    public interface IAssetsService
    {
        Task<AssetDto> GetByAssetTypeAsync(
            string assetTypeId, CancellationToken cancellationToken = default);
        
        Task<AssetDto> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<AssetSummaryDto>> GetAsync(
            string searchQuery,
            string orderBy,
            bool orderAsc,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<AssetDto> CreateAsync(CreateAssetDto assetDto, CancellationToken cancellationToken = default);

        Task<AssetDto> UpdateAsync(string id, UpdateAssetDto assetDto,
            CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}