using System;
using NomaNova.Ojeda.Client.Services.AssetClasses;
using NomaNova.Ojeda.Client.Services.Assets;
using NomaNova.Ojeda.Client.Services.Fields;
using NomaNova.Ojeda.Client.Services.FieldSets;

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
            FieldSetsService = new FieldSetsService(_httpClient);
            AssetClassesService = new AssetClassesService(_httpClient);
            AssetsService = new AssetsService(_httpClient);
        }

        public IFieldsService FieldsService { get; private set; }
        
        public IFieldSetsService FieldSetsService { get; private set; }

        public IAssetClassesService AssetClassesService { get; private set; }
        
        public IAssetsService AssetsService { get; private set; }

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