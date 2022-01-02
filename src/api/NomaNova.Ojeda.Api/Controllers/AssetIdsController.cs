using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.AssetIds;
using NomaNova.Ojeda.Services.AssetIds.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NomaNova.Ojeda.Api.Controllers;

[ApiController]
[Route("api/asset-ids")]
public class AssetIdsController : ApiController
{
    private const string Tag = "Asset Ids";
    
    private readonly IAssetIdsService _assetIdsService;
    
    public AssetIdsController(IAssetIdsService assetIdsService)
    {
        _assetIdsService = assetIdsService;
    }

    /// <summary>
    /// Generate asset id
    /// </summary>
    [HttpGet("generate")]
    [SwaggerOperation(Tags = new[] {Tag})]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GenerateAssetIdDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Generate(
        [FromQuery] string assetTypeId,
        CancellationToken cancellationToken = default)
    {
        var generateAssetId = await _assetIdsService.GenerateAssetId(assetTypeId, cancellationToken);
        return Ok(generateAssetId);
    }
}