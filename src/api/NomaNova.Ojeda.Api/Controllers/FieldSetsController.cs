using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;
using NomaNova.Ojeda.Services.FieldSets;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/fieldsets")]
    public class FieldSetsController : ApiController
    {
        private readonly IFieldSetsService _fieldSetsService;

        public FieldSetsController(IFieldSetsService fieldSetsService)
        {
            _fieldSetsService = fieldSetsService;
        }

        /// <summary>
        /// Get fieldset by id
        /// </summary>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var fieldSetDto = await _fieldSetsService.GetFieldSetByIdAsync(id, cancellationToken);
            return Ok(fieldSetDto);
        }

        /// <summary>
        /// Get all fieldsets
        /// </summary>
        /// <param name="query">Optional search query filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<FieldSetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query = null,
            [FromQuery] string orderBy = null,
            [FromQuery] bool orderAsc = true,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedFieldSetsDto = 
                await _fieldSetsService.GetFieldSetsAsync(query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedFieldSetsDto);
        }
        
        /// <summary>
        /// Create field set
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] FieldSetDto fieldSetDto, 
            CancellationToken cancellationToken = default)
        {
            fieldSetDto = await _fieldSetsService.CreateFieldSetAsync(fieldSetDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = fieldSetDto.Id }, fieldSetDto);
        }

        /// <summary>
        /// Update field set
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldSetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] FieldSetDto fieldSetDto,
            CancellationToken cancellationToken = default)
        {
            fieldSetDto = await _fieldSetsService.UpdateFieldSetAsync(id, fieldSetDto, cancellationToken);
            return Ok(fieldSetDto);
        }
        
        /// <summary>
        /// Delete field set
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            await _fieldSetsService.DeleteFieldSetAsync(id, cancellationToken);
            return Ok();
        }
    }
}