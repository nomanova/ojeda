using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
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
        /// Create field
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateFieldDto createFieldDto, 
            CancellationToken cancellationToken = default)
        {
            var fieldDto = await _fieldsService.CreateFieldAsync(createFieldDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = fieldDto.Id }, fieldDto);
        }

        /// <summary>
        /// Update field
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FieldDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] string id,
            [FromBody] UpdateFieldDto updateFieldDto,
            CancellationToken cancellationToken = default)
        {
            var fieldDto = await _fieldsService.UpdateFieldAsync(id, updateFieldDto, cancellationToken);
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