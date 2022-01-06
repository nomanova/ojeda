using Microsoft.AspNetCore.Http;

namespace NomaNova.Ojeda.Models.Dtos.AssetAttachments;

public class CreateAssetAttachmentDto
{
    public string AssetId { get; set; }

    public bool IsPrimary { get; set; }

    public IFormFile File { get; set; }
}