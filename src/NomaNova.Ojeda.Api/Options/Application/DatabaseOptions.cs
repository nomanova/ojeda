using System.ComponentModel.DataAnnotations;
using NomaNova.Ojeda.Api.Database;

namespace NomaNova.Ojeda.Api.Options.Application
{
    public class DatabaseOptions
    {
        [Required]
        public DatabaseType Type { get; set; }
        
        [Required]
        public string ConnectionString { get; set; }
    }
}