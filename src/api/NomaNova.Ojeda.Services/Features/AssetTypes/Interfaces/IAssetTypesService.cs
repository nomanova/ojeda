using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Features.AssetTypes.Interfaces;

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
        CreateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default);
        
    Task<AssetTypeDto> UpdateAsync(
        string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default);
        
    Task<DryRunUpdateAssetTypeDto> DryRunUpdateAsync(
        string id, UpdateAssetTypeDto assetTypeDto, CancellationToken cancellationToken = default);
        
    Task<DryRunDeleteAssetTypeDto> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken = default);
}