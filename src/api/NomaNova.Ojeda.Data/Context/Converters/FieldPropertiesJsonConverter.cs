using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Data.Context.Converters
{
    public class FieldPropertiesJsonConverter : JsonConverter
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

            var fieldProperties = jo.ToObject<FieldProperties>();

            if (fieldProperties == null)
            {
                throw new ArgumentOutOfRangeException(nameof(reader));
            }

            return fieldProperties.Type switch
            {
                FieldType.Text => jo.ToObject(typeof(TextFieldProperties), serializer),
                FieldType.Number => jo.ToObject(typeof(NumberFieldProperties), serializer),
                _ => throw new NotImplementedException(fieldProperties.Type.ToString())
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldProperties);
        }
    }
}