using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Core.Domain.AssetTypes
{
    public class AssetTypeFieldSet
    {
        public string FieldSetId { get; set; }
        
        public string AssetTypeId { get; set; }
        
        public uint Order { get; set; }
        
        public virtual FieldSet FieldSet { get; set; }
        
        public virtual AssetType AssetType { get; set; }
    }
}