using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Converters;

namespace NomaNova.Ojeda.Client.Services.Assets
{
    internal class AssetsService : BaseService, IAssetsService
    {
        private const string BasePath = "api/assets";

        public AssetsService(OjedaHttpClient httpClient) : base(httpClient)
        {
            JonConverters.Add(new FieldPropertiesDtoJsonConverter());
            JonConverters.Add(new FieldDataDtoJsonConverter());
        }

        public async Task<OjedaDataResult<AssetDto>> GetByIdAsync(string id,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<AssetDto>> GetByAssetType(
            string assetTypeId,
            CancellationToken cancellationToken = default)
        {
            var qsb = QueryStringBuilder.New();

            qsb.Add("assetTypeId", assetTypeId);

            var path = $"{BasePath}/new{qsb.Build()}";

            return await SendForDataAsync<AssetDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<PaginatedListDto<AssetSummaryDto>>> GetAsync(
            string query = null,
            string orderBy = null,
            bool orderAsc = true,
            int pageNumber = Constants.DefaultPageNumber,
            int pageSize = Constants.DefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            var qsb = QueryStringBuilder.New();

            qsb.Add("query", query);
            qsb.Add("orderBy", orderBy);
            qsb.Add("orderAsc", orderAsc);
            qsb.Add("pageNumber", pageNumber);
            qsb.Add("pageSize", pageSize);

            var path = $"{BasePath}{qsb.Build()}";

            return await SendForDataAsync<PaginatedListDto<AssetSummaryDto>>(HttpMethod.Get, path, null,
                cancellationToken);
        }

        public async Task<OjedaDataResult<AssetDto>> CreateAsync(CreateAssetDto asset,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Post, path, asset, cancellationToken);
        }

        public async Task<OjedaDataResult<AssetDto>> UpdateAsync(string id, UpdateAssetDto asset,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Put, path, asset, cancellationToken);
        }

        public async Task<OjedaResult> PatchAsync(string id, JsonPatchDocument<PatchAssetDto> patch,
            CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";  
            return await SendAsync(HttpMethod.Patch, path, patch, cancellationToken);
        }

        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}