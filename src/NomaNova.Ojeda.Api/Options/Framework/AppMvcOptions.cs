using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Core.Helpers;
using NomaNova.Ojeda.Services;

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
            options.SerializerSettings.ContractResolver = Serializer.JsonSettings.ContractResolver;
            options.SerializerSettings.Culture = Serializer.JsonSettings.Culture;
            options.SerializerSettings.DateFormatString = Serializer.JsonSettings.DateFormatString;
            options.SerializerSettings.ReferenceLoopHandling = Serializer.JsonSettings.ReferenceLoopHandling;
            options.SerializerSettings.Converters = Serializer.JsonSettings.Converters;
        }
    }
}