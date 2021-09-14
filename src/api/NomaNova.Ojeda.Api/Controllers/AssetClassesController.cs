using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;
using NomaNova.Ojeda.Services.AssetClasses;
using Swashbuckle.AspNetCore.Annotations;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/assetclasses")]
    public class AssetClassesController : ApiController
    {
        private const string Tag = "Asset Classes";
        
        private readonly IAssetClassesService _assetClassesService;

        public AssetClassesController(IAssetClassesService assetClassesService)
        {
            _assetClassesService = assetClassesService;
        }

        /// <summary>
        /// Get asset class by id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetClassDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var assetClassDto = await _assetClassesService.GetByIdAsync(id, cancellationToken);
            return Ok(assetClassDto);
        }

        /// <summary>
        /// Get all asset classes
        /// </summary>
        /// <param name="query">Optional search query filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<AssetClassDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query = null,
            [FromQuery] string orderBy = null,
            [FromQuery] bool orderAsc = true,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedAssetClassesDto = await _assetClassesService.GetAsync(
                query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedAssetClassesDto);
        }

        /// <summary>
        /// Create asset class
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetClassDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] AssetClassDto assetClassDto,
            CancellationToken cancellationToken = default)
        {
            assetClassDto = await _assetClassesService.CreateAsync(assetClassDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new {id = assetClassDto.Id}, assetClassDto);
        }

        /// <summary>
        /// Update asset class
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AssetClassDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] AssetClassDto assetClassDto,
            CancellationToken cancellationToken = default)
        {
            assetClassDto = await _assetClassesService.UpdateAsync(id, assetClassDto, cancellationToken);
            return Ok(assetClassDto);
        }

        /// <summary>
        /// Delete asset class
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            await _assetClassesService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}