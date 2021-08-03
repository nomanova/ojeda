using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NomaNova.Ojeda.Api.FileStore.Interfaces;
using NomaNova.Ojeda.Api.Options.Application;

namespace NomaNova.Ojeda.Api.FileStore
{
    public class FileSystem : IFileStore
    {
        private readonly FileSystemOptions _options;
        
        public FileSystem(IOptions<FileSystemOptions> options)
        {
            _options = options.Value;
        }

        public async Task WriteTextAsync(string path, string contents, CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(Path.Combine(_options.HomeDirectory, path), contents, cancellationToken);
        }

        public async Task WriteBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
        {
            await File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }

        public async Task<string> ReadTextAsync(string path, CancellationToken cancellationToken)
        {
            return await File.ReadAllTextAsync(Path.Combine(_options.HomeDirectory, path), cancellationToken);
        }
        
        public async Task<byte[]> ReadBytesAsync(string path, CancellationToken cancellationToken)
        {
            return await File.ReadAllBytesAsync(Path.Combine(_options.HomeDirectory, path), cancellationToken);
        }

        public async Task DeleteAsync(string path, CancellationToken cancellationToken)
        {
            var absPath = Path.Combine(_options.HomeDirectory, path);

            if (File.Exists(absPath))
            {
                await Task.Run(() => File.Delete(absPath), cancellationToken);
            }
        }
    }
}