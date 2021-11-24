using System;

namespace NomaNova.Ojeda.Api.Utils
{
    public static class EnvUtils
    {
        public static bool IsTesting()
        {
            return IsEnvironment(Constants.EnvTest);
        }
        
        private static bool IsEnvironment(string name)
        {
            var environmentName = Environment.GetEnvironmentVariable(Constants.AspNetCoreEnvironmentVar);
            return !string.IsNullOrEmpty(environmentName) &&
                   environmentName.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}