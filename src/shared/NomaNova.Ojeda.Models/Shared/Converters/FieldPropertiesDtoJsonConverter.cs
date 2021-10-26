using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Shared.Converters
{
    public class FieldPropertiesDtoJsonConverter : JsonConverter
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

            var fieldProperties = jo.ToObject<FieldPropertiesDto>();

            if (fieldProperties == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return fieldProperties.Type switch
            {
                FieldTypeDto.Text => jo.ToObject(typeof(TextFieldPropertiesDto), serializer),
                FieldTypeDto.Number => jo.ToObject(typeof(NumberFieldPropertiesDto), serializer),
                _ => throw new NotImplementedException(fieldProperties.Type.ToString())
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldPropertiesDto);
        }
    }
}