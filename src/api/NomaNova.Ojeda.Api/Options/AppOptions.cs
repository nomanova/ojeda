using NomaNova.Ojeda.Data.Options;
using NomaNova.Ojeda.Api.Options.Application;

namespace NomaNova.Ojeda.Api.Options
{
    public class AppOptions
    {
        public GlobalOptions Global { get; set; }

        public FileStoreOptions FileStore { get; set; }

        public DatabaseOptions Database { get; set; }

        public SecurityOptions Security { get; set; }
    }
}