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

        public (FieldPropertiesDto properties, bool isRequired) Resolve(string fieldId)
        {
            var assetField = _assetDto.FieldSets
                .SelectMany(fs => fs.Fields)
                .FirstOrDefault(f => f.Id.Equals(fieldId));
            
            if (assetField == null)
            {
                throw new ArgumentException($"Invalid state, field {fieldId} unknown.");
            }

            return (assetField.Properties, assetField.IsRequired);
        }
    }
}