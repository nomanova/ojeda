using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.AssetIdTypes;

public interface IAssetIdTypesService
{
    Task<OjedaDataResult<AssetIdTypeDto>> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<OjedaDataResult<PaginatedListDto<AssetIdTypeDto>>> GetAsync(
        string query = null,
        string orderBy = null,
        bool orderAsc = true,
        IEnumerable<string> excludedIds = null,
        int pageNumber = Constants.DefaultPageNumber,
        int pageSize = Constants.DefaultPageSize,
        CancellationToken cancellationToken = default);

    Task<OjedaDataResult<AssetIdTypeDto>> CreateAsync(CreateAssetIdTypeDto assetIdType,
        CancellationToken cancellationToken = default);

    Task<OjedaDataResult<AssetIdTypeDto>> UpdateAsync(string id, UpdateAssetIdTypeDto assetIdType,
        CancellationToken cancellationToken = default);

    Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task<OjedaDataResult<DryRunDeleteAssetIdTypeDto>> DryRunDeleteAsync(string id,
        CancellationToken cancellationToken = default);
}