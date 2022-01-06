using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Features.Assets.Interfaces;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ApiController
    {
        private readonly IAssetsService _assetsService;
        
        public AssetsController(IAssetsService assetsService)
        {
            _assetsService = assetsService;
        }
        
        /// <summary>
        /// Get asset by id
        /// </summary>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var assetDto = await _assetsService.GetByIdAsync(id, cancellationToken);
            return Ok(assetDto);
        }
        
        /// <summary>
        /// Get asset scaffold by asset type id
        /// </summary>
        [HttpGet("new")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAssetType(
            [FromQuery] string assetTypeId,
            CancellationToken cancellationToken = default)
        {
            var assetDto = await _assetsService.GetByAssetTypeAsync(assetTypeId, cancellationToken);
            return Ok(assetDto);
        }
        
        /// <summary>
        /// Get all assets
        /// </summary>
        /// <param name="query">Optional search query, filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<AssetSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query  = null,
            [FromQuery] string orderBy  = null,
            [FromQuery] bool orderAsc  = true,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedAssetsDto = await _assetsService.GetAsync(
                query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedAssetsDto);
        }
        
        /// <summary>
        /// Create asset
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateAssetDto createAssetDto,
            CancellationToken cancellationToken = default)
        {
            var assetDto = await _assetsService.CreateAsync(createAssetDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = assetDto.Id }, assetDto);
        }
        
        /// <summary>
        /// Update asset
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateAssetDto updateAssetDto,
            CancellationToken cancellationToken = default)
        {
            var assetDto = await _assetsService.UpdateAsync(id, updateAssetDto, cancellationToken);
            return Ok(assetDto);
        }

        /// <summary>
        /// Patch asset
        /// </summary>
        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(
            [FromRoute] string id,
            [FromBody] JsonPatchDocument<PatchAssetDto> patch,
            CancellationToken cancellationToken = default)
        {
            await _assetsService.PatchAsync(id, patch, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete asset
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            await _assetsService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}