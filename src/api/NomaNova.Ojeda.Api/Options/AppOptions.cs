using NomaNova.Ojeda.Api.Options.Application;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Data.Options;

namespace NomaNova.Ojeda.Api.Options
{
    public class AppOptions
    {
        public GlobalOptions Global { get; set; }

        public FileStoreOptions FileStore { get; set; }

        public DatabaseOptions Database { get; set; }
    }
}