using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;
using NomaNova.Ojeda.Models.Dtos.AssetAttachments;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Services.Features.AssetAttachments.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace NomaNova.Ojeda.Api.Controllers;

[ApiController]
[Route("api/asset-attachments")]
public class AssetAttachmentsController : ApiController
{
    private const string Tag = "Asset Attachments";

    private readonly IAssetAttachmentsService _assetAttachmentsService;

    public AssetAttachmentsController(IAssetAttachmentsService assetAttachmentsService)
    {
        _assetAttachmentsService = assetAttachmentsService;
    }

    /// <summary>
    /// Get asset attachment by id
    /// </summary>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AssetAttachmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] string id,
        CancellationToken cancellationToken = default)
    {
        var assetAttachmentDto = await _assetAttachmentsService.GetByIdAsync(id, cancellationToken);
        return Ok(assetAttachmentDto);
    }

    /// <summary>
    /// Get asset attachment raw data file
    /// </summary>
    [HttpGet("{id}/raw")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdRaw(
        [FromRoute] string id,
        CancellationToken cancellationToken = default)
    {
        var download = await _assetAttachmentsService.GetAsDownloadAsync(id, cancellationToken);
        return PhysicalFile(download.AbsolutePath, download.ContentType, download.FileName);
    }

    /// <summary>
    /// Create asset attachment
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Tags = new[] { Tag })]
    [Consumes(Constants.ContentTypeMultipartFormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AssetAttachmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromForm] CreateAssetAttachmentDto createAssetAttachment,
        CancellationToken cancellationToken = default)
    {
        var assetAttachmentDto = await _assetAttachmentsService.CreateAsync(createAssetAttachment, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = assetAttachmentDto.Id }, assetAttachmentDto);
    }

    /// <summary>
    /// Delete asset attachment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _assetAttachmentsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}