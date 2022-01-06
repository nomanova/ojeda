using System;

namespace NomaNova.Ojeda.Models.Dtos.AssetAttachments;

public class AssetAttachmentDto
{
    public string Id { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string AssetId { get; set; }
    
    public string DisplayFileName { get; set; }
    
    public string ContentType { get; set; }
    
    public long SizeInBytes { get; set; }
    
    public bool IsPrimary { get; set; }
}