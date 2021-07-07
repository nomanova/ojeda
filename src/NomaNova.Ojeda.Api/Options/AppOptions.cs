using NomaNova.Ojeda.Api.Options.Application;

namespace NomaNova.Ojeda.Api.Options
{
    public class AppOptions
    {
        public GlobalOptions Global { get; set; }
        
        public DatabaseOptions Database { get; set; }
    }
}