using System.ComponentModel.DataAnnotations;
using NomaNova.Ojeda.Utils.Attributes;

namespace NomaNova.Ojeda.Services.Shared.FileStore;

public class FileStoreOptions
{
    [Required]
    [Range(1048576, 104857600)] // 1MB - 100MB
    public long MaxSizeInBytes { get; set; }
    
    [Required]
    public FileStoreType Type { get; set; }

    public FileSystemOptions FileSystem { get; set; }
}

public class FileSystemOptions
{
    [Required]
    [Directory]
    public string RootDirectory { get; set; }
}