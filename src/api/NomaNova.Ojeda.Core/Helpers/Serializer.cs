using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NomaNova.Ojeda.Core.Helpers.Interfaces;

namespace NomaNova.Ojeda.Core.Helpers
{
    public class Serializer : ISerializer
    {
        public static readonly JsonSerializerSettings JsonSettings;
        
        private const string DefaultCulture = "en-US";
        private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"; // ISO 8601
        
        static Serializer()
        {
            JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Culture = new System.Globalization.CultureInfo(DefaultCulture),
                DateFormatString = DefaultDateTimeFormat,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonSettings.Converters.Add(new StringEnumConverter());
        }
                
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }
        
        public T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj, JsonSettings);
        }
    }
}