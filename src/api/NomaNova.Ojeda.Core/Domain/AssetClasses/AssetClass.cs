using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Assets;

namespace NomaNova.Ojeda.Core.Domain.AssetClasses
{
    public class AssetClass : BaseEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Description { get; set; }
        
        public virtual ICollection<AssetClassFieldSet> AssetClassFieldSets { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}