using System.Collections.Generic;

namespace NomaNova.Ojeda.Services.Shared.FileStore;

public class FileType
{
    public string Extension { get; set; }
    
    public string ContentType { get; set; }

    public bool IsImage { get; set; }

    public List<byte[]> Signatures { get; set; }
}