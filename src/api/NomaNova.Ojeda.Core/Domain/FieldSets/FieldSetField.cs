using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Core.Domain.FieldSets
{
    public class FieldSetField
    {
        public string FieldId { get; set; }

        public string FieldSetId { get; set; }

        public uint Order { get; set; }

        public virtual Field Field { get; set; }

        public virtual FieldSet FieldSet { get; set; }
    }
}