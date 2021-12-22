using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.AssetTypes;

namespace NomaNova.Ojeda.Core.Domain.AssetIdTypes;

public class AssetIdType : BaseEntity, INamedEntity, IDescribedEntity, ITimestampedEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Searchable]
    public string Name { get; set; }

    [Searchable]
    public string Description { get; set; }

    public bool WithManualEntry { get; set; }
    
    public SymbologyProperties Properties { get; set; }
    
    public virtual ICollection<AssetType> AssetTypes { get; set; }
}