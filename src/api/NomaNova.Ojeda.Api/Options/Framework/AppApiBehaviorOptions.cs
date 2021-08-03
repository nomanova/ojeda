using Microsoft.AspNetCore.Mvc;

namespace NomaNova.Ojeda.Api.Options.Framework
{
    public static class AppApiBehaviorOptions
    {
        public static void Apply(ApiBehaviorOptions options)
        {
            options.SuppressModelStateInvalidFilter = true;
            options.SuppressMapClientErrors = true;
        }
    }
}