using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Services.Shared.FileStore.Interfaces;

namespace NomaNova.Ojeda.Services.Shared.FileStore;

public class FileSystem : IFileStore
{
    private readonly FileSystemOptions _options;

    public FileSystem(IOptions<FileSystemOptions> options)
    {
        _options = options.Value;
    }

    public string ToAbsolutePath(string path)
    {
        return Path.Combine(_options.RootDirectory, path);
    }

    public async Task WriteTextAsync(string path, string contents, CancellationToken cancellationToken = default)
    {
        var fullPath = ToAbsolutePath(path);
        EnsureDirectory(fullPath);

        await File.WriteAllTextAsync(fullPath, contents, cancellationToken);
    }

    public async Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        var fullPath = ToAbsolutePath(path);
        EnsureDirectory(fullPath);

        await File.WriteAllBytesAsync(fullPath, bytes, cancellationToken);
    }

    public async Task<string> ReadTextAsync(string path, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllTextAsync(ToAbsolutePath(path), cancellationToken);
    }

    public async Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllBytesAsync(ToAbsolutePath(path), cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = ToAbsolutePath(path);

        if (File.Exists(fullPath))
        {
            await Task.Run(() => File.Delete(fullPath), cancellationToken);
        }
    }

    private static void EnsureDirectory(string fullPath)
    {
        var directoryPath = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}