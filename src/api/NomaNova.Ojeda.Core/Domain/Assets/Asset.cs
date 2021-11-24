using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.AssetTypes;

namespace NomaNova.Ojeda.Core.Domain.Assets
{
    public class Asset : BaseEntity, INamedEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        [Searchable]
        public string Name { get; set; }
        
        public string AssetTypeId { get; set; }
        
        public AssetType AssetType { get; set; }

        public virtual ICollection<FieldValue> FieldValues { get; set; }
    }
}