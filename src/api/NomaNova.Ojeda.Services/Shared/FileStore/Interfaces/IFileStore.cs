using System.Threading;
using System.Threading.Tasks;

namespace NomaNova.Ojeda.Services.Shared.FileStore.Interfaces;

public interface IFileStore
{
    string ToAbsolutePath(string path);
    
    Task WriteTextAsync(string path, string contents, CancellationToken cancellationToken = default);

    Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default);

    Task<string> ReadTextAsync(string path, CancellationToken cancellationToken = default);

    Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
}