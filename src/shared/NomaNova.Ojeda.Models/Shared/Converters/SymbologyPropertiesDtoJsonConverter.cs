using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

namespace NomaNova.Ojeda.Models.Shared.Converters;

public class SymbologyPropertiesDtoJsonConverter : JsonConverter
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

        var symbologyProperties = jo.ToObject<SymbologyPropertiesDto>();

        if (symbologyProperties == null)
        {
            throw new ArgumentOutOfRangeException(nameof(reader));
        }

        return symbologyProperties.Symbology switch
        {
            SymbologyDto.Ean13 => jo.ToObject(typeof(Ean13SymbologyPropertiesDto), serializer),
            _ => throw new NotImplementedException(symbologyProperties.Symbology.ToString())
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SymbologyPropertiesDto);
    }
}