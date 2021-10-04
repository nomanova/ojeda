using System.Collections.Generic;
using Newtonsoft.Json;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Utils.Services
{
    public class Serializer : ISerializer
    {
        public string Serialize(object obj, ICollection<JsonConverter> converters = null)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings.Get(converters));
        }
        
        public T Deserialize<T>(string obj, ICollection<JsonConverter> converters = null)
        {
            return JsonConvert.DeserializeObject<T>(obj, JsonSettings.Get(converters));
        }
    }
}