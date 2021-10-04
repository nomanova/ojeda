using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Shared.Converters
{
    public class FieldDataDtoJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var fieldData = jo.ToObject<FieldDataDto>();

            if (fieldData == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return fieldData.Type switch
            {
                FieldTypeDto.Text => jo.ToObject(typeof(TextFieldDataDto), serializer),
                FieldTypeDto.Number => jo.ToObject(typeof(NumberFieldDataDto), serializer),
                _ => throw new NotImplementedException(fieldData.Type.ToString())
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldDataDto);
        }
    }
}