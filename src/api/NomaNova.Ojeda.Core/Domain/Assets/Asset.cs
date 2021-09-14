using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.AssetClasses;

namespace NomaNova.Ojeda.Core.Domain.Assets
{
    public class Asset : BaseEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public string AssetClassId { get; set; }
        
        public AssetClass AssetClass { get; set; }

        public virtual ICollection<FieldValue> FieldValues { get; set; }
    }
}