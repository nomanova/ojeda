using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services.AssetTypes
{
    internal class AssetTypesService : BaseService, IAssetTypesService
    {
        private const string BasePath = "api/assettypes";
        
        public AssetTypesService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<OjedaDataResult<AssetTypeDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetTypeDto>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<PaginatedListDto<AssetTypeDto>>> GetAsync(
            string query,
            string orderBy, 
            bool orderAsc,
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            var qsb = QueryStringBuilder.New();
            
            qsb.Add("query" , query);
            qsb.Add("orderBy" , orderBy);
            qsb.Add("orderAsc" , orderAsc);
            qsb.Add("pageNumber" , pageNumber);
            qsb.Add("pageSize" , pageSize);

            var path = $"{BasePath}{qsb.Build()}";
            
            return await SendForDataAsync<PaginatedListDto<AssetTypeDto>>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<AssetTypeDto>> CreateAsync(CreateAssetTypeDto assetType, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<AssetTypeDto>(HttpMethod.Post, path, assetType, cancellationToken);
        }
        
        public async Task<OjedaDataResult<AssetTypeDto>> UpdateAsync(string id, UpdateAssetTypeDto assetType, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetTypeDto>(HttpMethod.Put, path, assetType, cancellationToken);
        }
        
        public async Task<OjedaDataResult<DeleteAssetTypeDto>> DeleteAsync(string id, bool dryRun, CancellationToken cancellationToken)
        {
            var qsb = QueryStringBuilder.New();
            
            qsb.Add("dryRun" , dryRun);
            
            var path = $"{BasePath}/{id}{qsb.Build()}";

            return await SendForDataAsync<DeleteAssetTypeDto>(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}