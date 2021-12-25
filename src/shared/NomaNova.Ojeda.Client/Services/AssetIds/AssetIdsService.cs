using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Client.Utils;
using NomaNova.Ojeda.Models.Dtos.AssetIds;

namespace NomaNova.Ojeda.Client.Services.AssetIds;

internal class AssetIdsService : BaseService, IAssetIdsService
{
    private const string BasePath = "api/asset-ids";
    
    public AssetIdsService(OjedaHttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<OjedaDataResult<GenerateAssetIdDto>> GenerateAsync(string assetTypeId,
        CancellationToken cancellationToken = default)
    {
        var qsb = QueryStringBuilder.New();

        qsb.Add("assetTypeId", assetTypeId);

        var path = $"{BasePath}/generate{qsb.Build()}";

        return await SendForDataAsync<GenerateAssetIdDto>(HttpMethod.Get, path, null, cancellationToken);
    }
}