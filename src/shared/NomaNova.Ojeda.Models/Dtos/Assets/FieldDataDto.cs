using System;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class FieldDataDto
    {
        public FieldDataTypeDto Type { get; set; }
    }

    public static class FieldDataDtoExtensions
    {
        public static FieldDataDto Copy(this FieldDataDto data)
        {
            return data.Type switch
            {
                FieldDataTypeDto.String => new StringFieldDataDto
                {
                    Value = ((StringFieldDataDto)data).Value
                },
                FieldDataTypeDto.Long => new LongFieldDataDto
                {
                    Value = ((LongFieldDataDto)data).Value
                },
                FieldDataTypeDto.Double => new DoubleFieldDataDto
                {
                    Value = ((DoubleFieldDataDto)data).Value
                },
                _ => throw new NotImplementedException(data.Type.ToString())
            };
        }
    }
}