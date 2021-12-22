using System;
using System.Linq;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public class AssetDtoFieldPropertiesResolver : IFieldPropertiesResolver
    {
        private readonly AssetDto _assetDto;

        public AssetDtoFieldPropertiesResolver(AssetDto assetDto)
        {
            _assetDto = assetDto;
        }

        public (FieldPropertiesDto properties, bool isRequired) Resolve(string fieldSetId, string fieldId)
        {
            var fieldSet = _assetDto.FieldSets.FirstOrDefault(_ => _.Id == fieldSetId);

            if (fieldSet == null)
            {
                throw new ArgumentException($"Invalid state, field set {fieldSetId} unknown.");
            }

            var field = fieldSet.Fields.FirstOrDefault(_ => _.Id == fieldId);
            
            if (field == null)
            {
                throw new ArgumentException($"Invalid state, field {fieldId} unknown.");
            }

            return (field.Properties, field.IsRequired);
        }
    }
}