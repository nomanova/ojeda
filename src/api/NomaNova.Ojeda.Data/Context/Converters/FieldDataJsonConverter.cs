using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Data.Context.Converters
{
    public class FieldDataJsonConverter : JsonConverter
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

            var fieldData = jo.ToObject<FieldData>();

            if (fieldData == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return fieldData.Type switch
            {
                FieldType.Text => jo.ToObject(typeof(TextFieldData), serializer),
                FieldType.Number => jo.ToObject(typeof(NumberFieldData), serializer),
                _ => throw new NotImplementedException(fieldData.Type.ToString())
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldData);
        }
    }
}