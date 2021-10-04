using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NomaNova.Ojeda.Utils
{
    public static class JsonSettings
    {
        private const string DefaultCulture = "en-US";
        private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ"; // ISO 8601
        
        private static JsonSerializerSettings Create()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false
                    }
                },
                Culture = new System.Globalization.CultureInfo(DefaultCulture),
                DateFormatString = DefaultDateTimeFormat,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            jsonSettings.Converters.Add(new StringEnumConverter());

            return jsonSettings;
        }
        
        public static JsonSerializerSettings Get(ICollection<JsonConverter> converters = null)
        {
            var settings = Create();

            if (converters != null)
            {
                foreach (var converter in converters)
                {
                    settings.Converters.Add(converter);
                }
            }

            return settings;
        }
    }
}