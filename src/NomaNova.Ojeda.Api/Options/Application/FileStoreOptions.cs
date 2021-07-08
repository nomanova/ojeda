using System.ComponentModel.DataAnnotations;
using NomaNova.Ojeda.Api.FileStore;
using NomaNova.Ojeda.Api.Utils;

namespace NomaNova.Ojeda.Api.Options.Application
{
    public class FileStoreOptions
    {
        [Required]
        public FileStoreType Type { get; set; }

        public FileSystemOptions FileSystem { get; set; }
    }

    public class FileSystemOptions
    {
        [Directory]
        public string HomeDirectory { get; set; }
    }
}