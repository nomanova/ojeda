using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Converters;

namespace NomaNova.Ojeda.Client.Services.AssetIdTypes;

internal class AssetIdTypesService : BaseService, IAssetIdTypesService
{
    private const string BasePath = "api/asset-id-types";
    
    public AssetIdTypesService(OjedaHttpClient httpClient) : base(httpClient)
    {
        JonConverters.Add(new SymbologyPropertiesDtoJsonConverter());
    }
    
    public async Task<OjedaDataResult<AssetIdTypeDto>> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var path = $"{BasePath}/{id}";
        return await SendForDataAsync<AssetIdTypeDto>(HttpMethod.Get, path, null, cancellationToken);
    }
    
    public async Task<OjedaDataResult<PaginatedListDto<AssetIdTypeDto>>> GetAsync(
        string query = null,
        string orderBy = null,
        bool orderAsc = true,
        IEnumerable<string> excludedIds = null,
        int pageNumber = Constants.DefaultPageNumber,
        int pageSize = Constants.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var qsb = QueryStringBuilder.New();

        qsb.Add("query", query);
        qsb.Add("orderBy", orderBy);
        qsb.Add("orderAsc", orderAsc);
        qsb.Add("excludedId", excludedIds);
        qsb.Add("pageNumber", pageNumber);
        qsb.Add("pageSize", pageSize);

        var path = $"{BasePath}{qsb.Build()}";

        return await SendForDataAsync<PaginatedListDto<AssetIdTypeDto>>(HttpMethod.Get, path, null, cancellationToken);
    }
    
    public async Task<OjedaDataResult<AssetIdTypeDto>> CreateAsync(CreateAssetIdTypeDto assetIdType,
        CancellationToken cancellationToken = default)
    {
        var path = $"{BasePath}";
        return await SendForDataAsync<AssetIdTypeDto>(HttpMethod.Post, path, assetIdType, cancellationToken);
    }
    
    public async Task<OjedaDataResult<AssetIdTypeDto>> UpdateAsync(string id, UpdateAssetIdTypeDto assetIdType,
        CancellationToken cancellationToken = default)
    {
        var path = $"{BasePath}/{id}";
        return await SendForDataAsync<AssetIdTypeDto>(HttpMethod.Put, path, assetIdType, cancellationToken);
    }
    
    public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var path = $"{BasePath}/{id}";
        return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
    }
}