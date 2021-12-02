using System;
using System.Text;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Services.Assets.Interfaces;

namespace NomaNova.Ojeda.Services.Assets
{
    /*
     * All field data is persisted in a string representation.
     * This allows searching over field values.
     * When a field has no value, it is stored as 'null'.
     */
    public class FieldDataConverter : IFieldDataConverter
    {
        public string ToStorage(FieldDataDto data, FieldProperties fieldProperties)
        {
            if (fieldProperties.Type == FieldType.Text)
            {
                if (data is not StringFieldDataDto stringData)
                {
                    throw new ArgumentException(null, nameof(data));
                }

                return string.IsNullOrEmpty(stringData.Value) ? null : stringData.Value;
            }

            if (fieldProperties.Type == FieldType.Number && !((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                if (data is not LongFieldDataDto longData)
                {
                    throw new ArgumentException(null, nameof(data));
                }

                return longData.Value?.ToString();
            }

            if (fieldProperties.Type == FieldType.Number && ((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                if (data is not DoubleFieldDataDto doubleData)
                {
                    throw new ArgumentException(null, nameof(data));
                }

                return doubleData.Value?.ToString();
            }

            throw new NotImplementedException(fieldProperties.Type.ToString());
        }

        public FieldDataDto FromStorage(string value, FieldProperties fieldProperties)
        {
            var isEmpty = string.IsNullOrEmpty(value);

            if (fieldProperties.Type == FieldType.Text)
            {
                return new StringFieldDataDto {Value = isEmpty ? null : value};
            }

            if (fieldProperties.Type == FieldType.Number && !((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                return new LongFieldDataDto {Value = isEmpty ? null : long.Parse(value)};
            }

            if (fieldProperties.Type == FieldType.Number && ((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                return new DoubleFieldDataDto {Value = isEmpty ? null : double.Parse(value)};
            }

            throw new NotImplementedException(fieldProperties.Type.ToString());
        }

        public FieldDataTypeDto GetMatchingDataType(FieldProperties fieldProperties)
        {
            if (fieldProperties.Type == FieldType.Text)
            {
                return FieldDataTypeDto.String;
            }

            if (fieldProperties.Type == FieldType.Number && !((NumberFieldProperties)fieldProperties).WithDecimals)
            {
                return FieldDataTypeDto.Long;
            }

            if (fieldProperties.Type == FieldType.Number && ((NumberFieldProperties)fieldProperties).WithDecimals)
            {
                return FieldDataTypeDto.Double;
            }
            
            throw new NotImplementedException(fieldProperties.Type.ToString());
        }
    }
}