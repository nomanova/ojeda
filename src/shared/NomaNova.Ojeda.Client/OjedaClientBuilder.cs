using System;

namespace NomaNova.Ojeda.Client
{
    public class OjedaClientBuilder
    {
        private readonly OjedaClientOptions _clientOptions = new();
        
        private readonly string _apiEndpoint;
        
        public OjedaClientBuilder(string apiEndpoint)
        {
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                throw new ArgumentNullException(nameof(apiEndpoint));
            }
            
            _apiEndpoint = apiEndpoint;
        }

        public OjedaClientBuilder WithRequestTimeout(TimeSpan requestTimeout)
        {
            _clientOptions.RequestTimeout = requestTimeout;
            return this;
        }
        
        public OjedaClient Build()
        {
            return new OjedaClient(_apiEndpoint, _clientOptions);
        }
    }
}