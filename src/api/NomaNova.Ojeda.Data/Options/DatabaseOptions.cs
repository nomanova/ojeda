using System.ComponentModel.DataAnnotations;

namespace NomaNova.Ojeda.Data.Options
{
    public class DatabaseOptions
    {
        [Required]
        public DatabaseType Type { get; set; }
        
        [Required]
        public string ConnectionString { get; set; }

        public bool RunSeeders { get; set; }
    }
}