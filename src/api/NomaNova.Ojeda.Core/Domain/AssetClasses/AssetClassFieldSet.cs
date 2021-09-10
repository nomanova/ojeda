using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Core.Domain.AssetClasses
{
    public class AssetClassFieldSet
    {
        public string FieldSetId { get; set; }
        
        public string AssetClassId { get; set; }
        
        public uint Order { get; set; }
        
        public virtual FieldSet FieldSet { get; set; }
        
        public virtual AssetClass AssetClass { get; set; }
    }
}