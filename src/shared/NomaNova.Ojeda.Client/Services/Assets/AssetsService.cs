using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Client.Services.Assets
{
    internal class AssetsService : BaseService, IAssetsService
    {
        private const string BasePath = "api/assets";
        
        public AssetsService(OjedaHttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<OjedaDataResult<AssetDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<AssetDto>> GetByAssetClass(
            string assetClassId,
            CancellationToken cancellationToken)
        {
            var qsb = QueryStringBuilder.New();
            
            qsb.Add("assetClassId" , assetClassId);
            
            var path = $"{BasePath}/new{qsb.Build()}";

            return await SendForDataAsync<AssetDto>(HttpMethod.Get, path, null, cancellationToken);
        }

        public async Task<OjedaDataResult<AssetDto>> CreateAsync(AssetDto asset, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Post, path, asset, cancellationToken);
        }
        
        public async Task<OjedaDataResult<AssetDto>> UpdateAsync(string id, AssetDto asset, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendForDataAsync<AssetDto>(HttpMethod.Put, path, asset, cancellationToken);
        }
        
        public async Task<OjedaResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var path = $"{BasePath}/{id}";
            return await SendAsync(HttpMethod.Delete, path, null, cancellationToken);
        }
    }
}