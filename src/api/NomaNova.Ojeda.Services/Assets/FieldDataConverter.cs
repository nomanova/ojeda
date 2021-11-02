using System;
using System.Text;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Services.Assets.Interfaces;

namespace NomaNova.Ojeda.Services.Assets
{
    public class FieldDataConverter : IFieldDataConverter
    {
        public byte[] ToBytes(FieldDataDto data, FieldProperties fieldProperties)
        {
            if (fieldProperties.Type == FieldType.Text)
            {
                if (!(data is StringFieldDataDto))
                {
                    throw new ArgumentException(nameof(data));
                }

                var stringData = (StringFieldDataDto) data;
                return Encoding.UTF8.GetBytes(stringData.Value ?? string.Empty);
            }

            if (fieldProperties.Type == FieldType.Number && !((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                if (!(data is LongFieldDataDto))
                {
                    throw new ArgumentException(nameof(data));
                }

                var longData = (LongFieldDataDto) data;
                return longData.Value.HasValue ? BitConverter.GetBytes(longData.Value.Value) : null;
            }

            if (fieldProperties.Type == FieldType.Number && ((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                if (!(data is DoubleFieldDataDto))
                {
                    throw new ArgumentException(nameof(data));
                }

                var doubleData = (DoubleFieldDataDto) data;
                return doubleData.Value.HasValue ? BitConverter.GetBytes(doubleData.Value.Value) : null;
            }

            throw new NotImplementedException(fieldProperties.Type.ToString());
        }

        public FieldDataDto FromBytes(byte[] value, FieldProperties fieldProperties)
        {
            var isEmpty = value == null || value.Length == 0;

            if (fieldProperties.Type == FieldType.Text)
            {
                return new StringFieldDataDto {Value = isEmpty ? null : Encoding.UTF8.GetString(value)};
            }

            if (fieldProperties.Type == FieldType.Number && !((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                return new LongFieldDataDto {Value = isEmpty ? null : BitConverter.ToInt64(value, 0)};
            }

            if (fieldProperties.Type == FieldType.Number && ((NumberFieldProperties) fieldProperties).WithDecimals)
            {
                return new DoubleFieldDataDto {Value = isEmpty ? null : BitConverter.ToDouble(value, 0)};
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