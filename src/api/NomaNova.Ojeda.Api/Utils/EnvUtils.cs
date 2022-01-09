using System;

namespace NomaNova.Ojeda.Api.Utils
{
    public static class EnvUtils
    {
        public static bool IsTesting()
        {
            return IsEnvironment(Constants.EnvTest);
        }
        
        public static bool IsDevelopment()
        {
            return IsEnvironment(Constants.EnvDevelopment);
        }
        
        public static bool IsProduction()
        {
            return IsEnvironment(Constants.EnvProduction);
        }
        
        private static bool IsEnvironment(string name)
        {
            var environmentName = Environment.GetEnvironmentVariable(Constants.AspNetCoreEnvironmentVar);
            return !string.IsNullOrEmpty(environmentName) &&
                   environmentName.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}