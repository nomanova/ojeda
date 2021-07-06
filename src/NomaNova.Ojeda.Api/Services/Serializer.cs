using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NomaNova.Ojeda.Api.Services
{
    public class Serializer : ISerializer
    {
        public static readonly JsonSerializerSettings JsonSettings;
        
        static Serializer()
        {
            JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Culture = new System.Globalization.CultureInfo(Constants.DefaultCulture),
                DateFormatString = Constants.DefaultDateTimeFormat,
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
        
        public static void ApplyJsonSettings(MvcNewtonsoftJsonOptions settings)
        {
            settings.SerializerSettings.ContractResolver = JsonSettings.ContractResolver;
            settings.SerializerSettings.Culture = JsonSettings.Culture;
            settings.SerializerSettings.DateFormatString = JsonSettings.DateFormatString;
            settings.SerializerSettings.ReferenceLoopHandling = JsonSettings.ReferenceLoopHandling;
            settings.SerializerSettings.Converters = JsonSettings.Converters;
        }
    }
}