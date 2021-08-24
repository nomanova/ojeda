using System;
using NomaNova.Ojeda.Client.Services.Fields;

namespace NomaNova.Ojeda.Client
{
    public sealed class OjedaClient : IDisposable
    {
        private readonly string _apiEndpoint;
        private readonly OjedaClientOptions _options;

        private OjedaHttpClient _httpClient;
        private bool _isDisposed;

        public OjedaClient(string apiEndpoint, OjedaClientOptions options)
        {
            _apiEndpoint = apiEndpoint;
            _options = options;

            Initialize();
        }

        private void Initialize()
        {
            var baseAddress = new Uri(_apiEndpoint);
            _httpClient = OjedaHttpClient.Create(baseAddress, _options.RequestTimeout);

            FieldsService = new FieldsService(_httpClient);
        }

        public IFieldsService FieldsService { get; private set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_httpClient != null)
            {
                try
                {
                    _httpClient.Dispose();
                }
                catch (Exception)
                {
                    // NOP: might happen for inflight request during client disposal
                }

                _httpClient = null;
            }

            _isDisposed = true;
        }
    }
}