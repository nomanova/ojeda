using Microsoft.AspNetCore.Cors.Infrastructure;
using NomaNova.Ojeda.Api.Options.Application;

namespace NomaNova.Ojeda.Api.Options.Framework
{
    public static class AppCorsOptions
    {
        public static void Apply(CorsOptions options, SecurityOptions securityOptions)
        {
            if (securityOptions != null)
            {
                options.AddPolicy(Constants.CorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(securityOptions.AllowedOrigins.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });                
            }
        }
    }
}