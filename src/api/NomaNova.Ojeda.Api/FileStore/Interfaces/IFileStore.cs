using System.Threading;
using System.Threading.Tasks;

namespace NomaNova.Ojeda.Api.FileStore.Interfaces
{
    public interface IFileStore
    {
        Task WriteTextAsync(string path, string contents, CancellationToken cancellationToken = default);

        Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default);

        Task<string> ReadTextAsync(string path, CancellationToken cancellationToken = default);

        Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken = default);

        Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    }
}