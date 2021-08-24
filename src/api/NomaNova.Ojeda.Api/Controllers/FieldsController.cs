using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Errors;
using NomaNova.Ojeda.Models.Fields;
using NomaNova.Ojeda.Services.Fields;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/fields")]
    public class FieldsController : ApiController
    {
        private readonly IFieldsService _fieldsService;
        
        public FieldsController(IFieldsService fieldsService)
        {
            _fieldsService = fieldsService;
        }

        /// <summary>
        /// Get field by id
        /// </summary>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            var fieldDto = await _fieldsService.GetFieldByIdAsync(id, cancellationToken);
            return Ok(fieldDto);
        }

        /// <summary>
        /// Get all fields
        /// </summary>
        /// <param name="query">Optional search query filtering results based on the searchable fields.</param>
        /// <param name="orderBy">Optional field name on which to order the results.</param>
        /// <param name="orderAsc">Optional ordering direction indication, default is ascending order.</param>
        /// <param name="pageNumber">Optional result page number.</param>
        /// <param name="pageSize">Optional result page size.</param>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedListDto<FieldDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string query  = null,
            [FromQuery] string orderBy  = null,
            [FromQuery] bool orderAsc  = true,
            [FromQuery] int pageNumber = Constants.DefaultQueryPageNumber,
            [FromQuery] int pageSize = Constants.DefaultQueryPageSize,
            CancellationToken cancellationToken = default)
        {
            var paginatedFieldsDto = 
                await _fieldsService.GetFieldsAsync(query, orderBy, orderAsc, pageNumber, pageSize, cancellationToken);
            return Ok(paginatedFieldsDto);
        }

        /// <summary>
        /// Create field
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] FieldDto fieldDto, 
            CancellationToken cancellationToken = default)
        {
            fieldDto = await _fieldsService.CreateFieldAsync(fieldDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = fieldDto.Id }, fieldDto);
        }

        /// <summary>
        /// Update field
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationErrorDto>),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] FieldDto fieldDto,
            CancellationToken cancellationToken = default)
        {
            fieldDto = await _fieldsService.UpdateFieldAsync(id, fieldDto, cancellationToken);
            return Ok(fieldDto);
        }

        /// <summary>
        /// Delete field
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            await _fieldsService.DeleteFieldAsync(id, cancellationToken);
            return Ok();
        }
    }
}