using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Utils.Services;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Client.Services
{
    internal abstract class BaseService
    {
        private readonly OjedaHttpClient _httpClient;
        private readonly ISerializer _serializer;
        
        protected IList<JsonConverter> JonConverters { get; } = new List<JsonConverter>();
        
        protected BaseService(OjedaHttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializer = new Serializer();
        }

        protected async Task<OjedaResult> SendAsync(
            HttpMethod method,
            string uri,
            object payload = default,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(method, uri);
            TryAddPayload(request, payload);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return OjedaResult.ForFailure(HttpStatusCode.ServiceUnavailable, new ErrorDto
                {
                    Code = (int) HttpStatusCode.ServiceUnavailable,
                    Message = ex.Message
                });
            }

            return await ParseResponseAsync(response);
        }

        protected async Task<OjedaDataResult<T>> SendForDataAsync<T>(
            HttpMethod method,
            string uri,
            object payload = default,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var request = new HttpRequestMessage(method, uri);
            TryAddPayload(request, payload);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return OjedaDataResult<T>.ForFailure(HttpStatusCode.ServiceUnavailable, new ErrorDto
                {
                    Code = (int) HttpStatusCode.ServiceUnavailable,
                    Message = ex.Message
                });
            }
            
            return await ParseDataResponseAsync<T>(response);
        }

        private  async Task<OjedaResult> ParseResponseAsync(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                var error = await TryParseErrorAsync(response);
                return OjedaResult.ForFailure(statusCode, error);
            }

            return OjedaResult.ForSuccess(statusCode);
        }

        private async Task<OjedaDataResult<T>> ParseDataResponseAsync<T>(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                var error = await TryParseErrorAsync(response);
                return OjedaDataResult<T>.ForFailure(statusCode, error);
            }

            var data = await GetPayloadAsync<T>(response);

            return OjedaDataResult<T>.ForSuccess(data, statusCode);
        }

        private async Task<T> GetPayloadAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<T>(content, JonConverters);
        }

        private async Task<ErrorDto> TryParseErrorAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            return _serializer.Deserialize<ErrorDto>(content, JonConverters);
        }

        private void TryAddPayload(HttpRequestMessage request, object payload)
        {
            if (payload == null)
            {
                return;
            }

            var body = _serializer.Serialize(payload, JonConverters);
            request.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}