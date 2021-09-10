using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;

namespace NomaNova.Ojeda.Client.Services.AssetClasses
{
    internal class AssetClassesService : BaseService, IAssetClassesService
    {
        private const string BasePath = "api/assetclasses";
        
        public AssetClassesService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<OjedaDataResult<AssetClassDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetClassDto>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<PaginatedListDto<AssetClassDto>>> GetAsync(
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
            
            return await SendForDataAsync<PaginatedListDto<AssetClassDto>>(HttpMethod.Get, path, null, cancellationToken);
        }
        
        public async Task<OjedaDataResult<AssetClassDto>> CreateAsync(AssetClassDto assetClass, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<AssetClassDto>(HttpMethod.Post, path, assetClass, cancellationToken);
        }
        
        public async Task<OjedaDataResult<AssetClassDto>> UpdateAsync(string id, AssetClassDto assetClass, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetClassDto>(HttpMethod.Put, path, assetClass, cancellationToken);
        }
        
        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}