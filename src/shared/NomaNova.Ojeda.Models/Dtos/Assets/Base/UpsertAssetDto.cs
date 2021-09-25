using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetDto <T, TS> 
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public string AssetTypeId { get; set; }

        public List<T> FieldSets { get; set; }
    }

    public abstract class UpsertAssetFieldSetDto<T> where T : UpsertAssetFieldDto
    {
        public string Id { get; set; }
        
        public List<T> Fields { get; set; }
    }

    public abstract class UpsertAssetFieldDto
    {
        public string Id { get; set; }

        public string Value { get; set; }
    }
}