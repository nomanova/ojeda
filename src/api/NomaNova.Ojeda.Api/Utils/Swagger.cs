using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace NomaNova.Ojeda.Api.Utils
{
    public static class Swagger
    {
        public static string GetDoc(string version = "v1")
        {
            return version;
        }
        
        public static string GetDefinition(string appName = Constants.AppName, string version = "v1")
        {
            return $"{appName} {version}";
        }

        public static string GetJsonPath(string version = "v1")
        {
            return $"/swagger/{version}/swagger.json";
        }

        public static string GetXmlPath(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            return Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
        }
        
        public static OpenApiInfo GetInfo(string appName = Constants.AppName, string version = "v1")
        {
            return new OpenApiInfo
            {
                Title = appName,
                Version = version,
                Description = $"The {appName} API specification.",
                Contact = new OpenApiContact {Name = Constants.CompanyName, Email = Constants.CompanyEmail}
            };
        }
    }
}