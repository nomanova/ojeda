using System;
using System.Collections.Generic;

namespace NomaNova.Ojeda.Core.Domain.FieldSets
{
    public class FieldSet : BaseEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Description { get; set; }

        public virtual ICollection<FieldSetField> FieldSetFields { get; set; }
    }
}