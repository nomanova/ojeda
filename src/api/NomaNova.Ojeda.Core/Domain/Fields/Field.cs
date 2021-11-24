using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class Field : BaseEntity, INamedEntity, IDescribedEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Description { get; set; }

        public FieldProperties Properties { get; set; }

        public virtual ICollection<FieldSetField> FieldSetFields { get; set; }
        
        public virtual ICollection<FieldValue> FieldValues { get; set; }
    }
}