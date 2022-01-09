using System.Collections.Generic;
using System.Linq;

namespace NomaNova.Ojeda.Services.Shared.FileStore;

/**
 * https://www.filesignatures.net
 */
public static class FileTypeProvider
{
    public const string ContentTypeImagePng = "image/png";
    public const string ContentTypeImageJpeg = "image/jpeg";
    public const string ContentTypeImageJpg = "image/jpg";
    
    public static readonly IList<FileType> FileTypes = new List<FileType>();

    static FileTypeProvider()
    {
        FileTypes.Add(new FileType
        {
            ContentType = ContentTypeImagePng,
            Extension = ".png",
            IsImage = true,
            Signatures = new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
        });

        FileTypes.Add(new FileType
        {
            ContentType = ContentTypeImageJpeg,
            Extension = ".jpeg",
            IsImage = true,
            Signatures = new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
            }
        });

        FileTypes.Add(new FileType
        {
            ContentType = ContentTypeImageJpg,
            Extension = ".jpg",
            IsImage = true,
            Signatures = new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
            }
        });
    }

    public static FileType ForExtension(string extension)
    {
        return FileTypes.FirstOrDefault(_ => _.Extension == extension);
    }
}