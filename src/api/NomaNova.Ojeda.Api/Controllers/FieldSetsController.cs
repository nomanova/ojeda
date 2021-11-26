using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.FieldSets.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/fieldsets")]
    public class FieldSetsController : ApiController
    {
        private const string Tag = "Field Sets";
        
        private readonly IFieldSetsService _fieldSetsService;

        public FieldSetsController(IFieldSetsService fieldSetsService)
        {
            _fieldSetsService = fieldSetsService;
        }

        /// <summary>
        /// Get field set by id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var fieldSetDto = await _fieldSetsService.GetByIdAsync(id, cancellationToken);
            return Ok(fieldSetDto);
        }

        /// <summary>
        /// Get all field sets
        /// </summary>
        /// <param name="query">Optional search query filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="excludedIds">Optional list of field set id's which must be excluded from the result set.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<FieldSetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query = null,
            [FromQuery] string orderBy = null,
            [FromQuery] bool orderAsc = true,
            [FromQuery(Name = "excludedId")] IList<string> excludedIds = null,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedFieldSetsDto = await _fieldSetsService.GetAsync(
                query, orderBy, orderAsc, excludedIds, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedFieldSetsDto);
        }
        
        /// <summary>
        /// Create field set
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateFieldSetDto createFieldSetDto, 
            CancellationToken cancellationToken = default)
        {
            var fieldSetDto = await _fieldSetsService.CreateAsync(createFieldSetDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = fieldSetDto.Id }, fieldSetDto);
        }

        /// <summary>
        /// Update field set
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateFieldSetDto updateFieldSetDto,
            CancellationToken cancellationToken = default)
        {
            var fieldSetDto = await _fieldSetsService.UpdateAsync(id, updateFieldSetDto, cancellationToken);
            return Ok(fieldSetDto);
        }
        
        /// <summary>
        /// Delete field set
        /// </summary>
        /// <param name="dryRun">When true only the impact of deletion will be calculated.</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] {Tag})]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(DeleteFieldSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] string id,
            [FromQuery] bool dryRun,
            CancellationToken cancellationToken = default)
        {
            var deleteFieldSetDto = await _fieldSetsService.DeleteAsync(id, dryRun, cancellationToken);
            return Ok(deleteFieldSetDto);
        }
    }
}