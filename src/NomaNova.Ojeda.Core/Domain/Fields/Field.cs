using System;

namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class Field : BaseEntity, ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public FieldType Type { get; set; }
    }
}