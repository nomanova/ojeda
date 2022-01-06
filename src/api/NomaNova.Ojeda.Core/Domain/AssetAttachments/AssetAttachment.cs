using System;
using NomaNova.Ojeda.Core.Domain.Assets;

namespace NomaNova.Ojeda.Core.Domain.AssetAttachments;

public class AssetAttachment : BaseEntity, ITimestampedEntity
{
    public DateTime CreatedAt { get; set; }
        
    public DateTime UpdatedAt { get; set; }
    
    public string AssetId { get; set; }
        
    public Asset Asset { get; set; }

    public string DisplayFileName { get; set; }
    
    public string StorageFileName { get; set; }

    public string ContentType { get; set; }

    public long SizeInBytes { get; set; }

    public bool IsPrimary { get; set; }

    public byte[] Thumbnail { get; set; }
}