using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using CacheControlHeaderValue = System.Net.Http.Headers.CacheControlHeaderValue;

namespace NomaNova.Ojeda.Client
{
    internal sealed class OjedaHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private bool _disposedValue;

        private OjedaHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static OjedaHttpClient Create(Uri baseAddress, TimeSpan? requestTimeout)
        {
            var httpMessageHandler = new HttpClientHandler();

            var httpClient = new HttpClient(httpMessageHandler)
            {
                BaseAddress = baseAddress,
                Timeout = requestTimeout ?? TimeSpan.FromSeconds(5)
            };

            httpClient.DefaultRequestHeaders.CacheControl =
                new CacheControlHeaderValue {NoCache = true};
            
            httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, MediaTypeNames.Application.Json);

            return new OjedaHttpClient(httpClient);
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            
            return await _httpClient.SendAsync(request, cancellationToken);
        }

        private void ThrowIfDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(OjedaHttpClient));
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposedValue = true;
        }
    }
}