using System;

namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class Field : BaseEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Description { get; set; }

        public FieldType Type { get; set; }
    }
}