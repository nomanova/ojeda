using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class Field : BaseEntity, INamedEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Description { get; set; }

        public FieldType Type { get; set; }
        
        public virtual ICollection<FieldSetField> FieldSetFields { get; set; }
    }
}