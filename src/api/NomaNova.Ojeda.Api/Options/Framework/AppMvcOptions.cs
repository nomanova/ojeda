using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NomaNova.Ojeda.Models.Shared.Converters;
using NomaNova.Ojeda.Utils;

namespace NomaNova.Ojeda.Api.Options.Framework
{
    public static class AppMvcOptions
    {
        public static void Apply(MvcOptions options)
        {
            // NOP
        }

        public static void Apply(MvcNewtonsoftJsonOptions options)
        {
            var jsonConverters = new List<JsonConverter>
            {
                new FieldDataDtoJsonConverter()
            };
            
            var settings = JsonSettings.Get(jsonConverters);
            
            options.SerializerSettings.ContractResolver = settings.ContractResolver;
            options.SerializerSettings.Culture = settings.Culture;
            options.SerializerSettings.DateFormatString = settings.DateFormatString;
            options.SerializerSettings.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            options.SerializerSettings.Converters = settings.Converters;
        }
    }
}