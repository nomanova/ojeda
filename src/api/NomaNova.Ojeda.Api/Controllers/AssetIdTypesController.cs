using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.AssetIdTypes.Interfaces;

namespace NomaNova.Ojeda.Api.Controllers;

[ApiController]
[Route("api/asset-id-types")]
public class AssetIdTypesController : ApiController
{
    private readonly IAssetIdTypesService _assetIdTypesService;

    public AssetIdTypesController(IAssetIdTypesService assetIdTypesService)
    {
        _assetIdTypesService = assetIdTypesService;
    }

    /// <summary>
    /// Get asset id type by id
    /// </summary>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AssetIdTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] string id,
        CancellationToken cancellationToken = default)
    {
        var fieldDto = await _assetIdTypesService.GetByIdAsync(id, cancellationToken);
        return Ok(fieldDto);
    }

    /// <summary>
    /// Get all asset id types
    /// </summary>
    /// <param name="query">Optional search query, filtering results based on the searchable fields.</param>
    /// <param name="orderBy">Optional field name on which to order the results.</param>
    /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
    /// <param name="excludedIds">Optional list of field id's which must be excluded from the result set.</param>
    /// <param name="pageNumber">Optional result page number.</param>
    /// <param name="pageSize">Optional result page size.</param>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedListDto<AssetIdTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] string query = null,
        [FromQuery] string orderBy = null,
        [FromQuery] bool orderAsc = true,
        [FromQuery(Name = "excludedId")] IList<string> excludedIds = null,
        [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
        [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
        CancellationToken cancellationToken = default)
    {
        var paginatedAssetIdTypesDto = await _assetIdTypesService.GetAsync(
            query, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);
        return Ok(paginatedAssetIdTypesDto);
    }

    /// <summary>
    /// Create asset id type
    /// </summary>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AssetIdTypeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetIdTypeDto createAssetIdTypeDto,
        CancellationToken cancellationToken = default)
    {
        var assetIdTypeDto = await _assetIdTypesService.CreateAsync(createAssetIdTypeDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = assetIdTypeDto.Id }, assetIdTypeDto);
    }

    /// <summary>
    /// Update asset id type
    /// </summary>
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AssetIdTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] UpdateAssetIdTypeDto updateAssetIdTypeDto,
        CancellationToken cancellationToken = default)
    {
        var assetIdTypeDto = await _assetIdTypesService.UpdateAsync(id, updateAssetIdTypeDto, cancellationToken);
        return Ok(assetIdTypeDto);
    }

    /// <summary>
    /// Delete asset id type
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _assetIdTypesService.DeleteAsync(id, false, cancellationToken);
        return Ok();
    }
    
    /// <summary>
    /// Delete asset id type (dry run)
    /// </summary>
    [HttpDelete("{id}/dry-run")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DryRunDeleteAssetIdTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DryRunDelete(
        [FromRoute] string id,
        CancellationToken cancellationToken = default)
    {
        var deleteAssetIdTypeDto = await _assetIdTypesService.DeleteAsync(id, true, cancellationToken);
        return Ok(deleteAssetIdTypeDto);
    }
}