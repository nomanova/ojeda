using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;

namespace NomaNova.Ojeda.Data.Context.Converters;

public class SymbologyPropertiesJsonConverter : JsonConverter
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

        var symbologyProperties = jo.ToObject<SymbologyProperties>();
        
        if (symbologyProperties == null)
        {
            throw new ArgumentOutOfRangeException(nameof(reader));
        }

        return symbologyProperties.Symbology switch
        {
            Symbology.Ean13 => jo.ToObject(typeof(Ean13SymbologyProperties), serializer),
            _ => throw new NotImplementedException(symbologyProperties.Symbology.ToString())
        };
    }
    
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SymbologyProperties);
    }
}