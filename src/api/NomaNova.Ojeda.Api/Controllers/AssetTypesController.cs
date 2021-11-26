using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.AssetTypes.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/assettypes")]
    public class AssetTypesController : ApiController
    {
        private const string Tag = "Asset Types";
        
        private readonly IAssetTypesService _assetTypesService;

        public AssetTypesController(IAssetTypesService assetTypesService)
        {
            _assetTypesService = assetTypesService;
        }

        /// <summary>
        /// Get asset type by id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var assetTypeDto = await _assetTypesService.GetByIdAsync(id, cancellationToken);
            return Ok(assetTypeDto);
        }

        /// <summary>
        /// Get all asset types
        /// </summary>
        /// <param name="query">Optional search query filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<AssetTypeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query = null,
            [FromQuery] string orderBy = null,
            [FromQuery] bool orderAsc = true,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedAssetTypesDto = await _assetTypesService.GetAsync(
                query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedAssetTypesDto);
        }

        /// <summary>
        /// Create asset type
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateAssetTypeDto createAssetTypeDto,
            CancellationToken cancellationToken = default)
        {
            var assetTypeDto = await _assetTypesService.CreateAsync(createAssetTypeDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new {id = assetTypeDto.Id}, assetTypeDto);
        }

        /// <summary>
        /// Update asset type
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateAssetTypeDto updateAssetTypeDto,
            CancellationToken cancellationToken = default)
        {
            var assetTypeDto = await _assetTypesService.UpdateAsync(id, updateAssetTypeDto, cancellationToken);
            return Ok(assetTypeDto);
        }

        /// <summary>
        /// Delete asset type
        /// </summary>
        /// <param name="dryRun">When true only the impact of deletion will be calculated.</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(DeleteAssetTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] string id, 
            [FromQuery] bool dryRun,
            CancellationToken cancellationToken = default)
        {
            var deleteAssetTypeDto = await _assetTypesService.DeleteAsync(id, dryRun, cancellationToken);
            return Ok(deleteAssetTypeDto);
        }
    }
}