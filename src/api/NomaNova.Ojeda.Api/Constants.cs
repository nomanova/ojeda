namespace NomaNova.Ojeda.Api
{
    public static class Constants
    {
        public const string AppName = "Ojeda";
        
        public const string CompanyName = "nomanova";
        public const string CompanyEmail = "hello@nomanova.com";
        
        public const string EnvProduction = "Production";
        public const string EnvDevelopment = "Development";
        
        public const int ExitOk = 0;
        public const int ExitError = 1;
        
        public const string LogTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
    }
}