using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Core.Domain.Assets
{
    public class FieldValue : BaseEntity
    {
        public string AssetId { get; set; }
        
        public Asset Asset { get; set; }

        public string FieldSetId { get; set; }

        public FieldSet FieldSet { get; set; }

        public string FieldId { get; set; }

        public Field Field { get; set; }

        public string Value { get; set; }
    }
}