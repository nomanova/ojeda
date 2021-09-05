using System;
using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Core.Domain.FieldSets
{
    public class FieldSetField : ITimestampedEntity
    {
        public string FieldId { get; set; }

        public string FieldSetId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int Order { get; set; }

        public virtual Field Field { get; set; }

        public virtual FieldSet FieldSet { get; set; }
    }
}