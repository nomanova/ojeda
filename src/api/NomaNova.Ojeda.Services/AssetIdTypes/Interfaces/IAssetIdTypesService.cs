using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.AssetIdTypes.Interfaces;

public interface IAssetIdTypesService
{
    Task<AssetIdTypeDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<PaginatedListDto<AssetIdTypeDto>> GetAsync(
        string searchQuery,
        string orderBy,
        bool orderAsc,
        IList<string> excludedIds,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<AssetIdTypeDto> CreateAsync(CreateAssetIdTypeDto assetIdTypeDto,
        CancellationToken cancellationToken = default);

    Task<AssetIdTypeDto> UpdateAsync(string id, UpdateAssetIdTypeDto assetIdTypeDto,
        CancellationToken cancellationToken = default);

    Task<DryRunDeleteAssetIdTypeDto> DeleteAsync(string id, bool dryRun,
        CancellationToken cancellationToken = default);
}