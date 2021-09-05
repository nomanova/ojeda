using System.Net.Http;
using System.Net.Mime;
using System.Text;
using NomaNova.Ojeda.Core.Helpers.Interfaces;

namespace NomaNova.Ojeda.Api.Tests.Controllers.Base
{
    public class RequestBuilder
    {
        private readonly HttpRequestMessage _request;
        
        public RequestBuilder(string path) : this(HttpMethod.Get, path)
        {
        }
        
        public RequestBuilder(HttpMethod method, string path)
        {
            _request = new HttpRequestMessage(method, path);
        }
        
        public RequestBuilder WithPayload(ISerializer serializer, object payload)
        {
            var body = serializer.Serialize(payload);
            _request.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
            return this;
        }
        
        public HttpRequestMessage Build()
        {
            return _request;
        }
    }
}