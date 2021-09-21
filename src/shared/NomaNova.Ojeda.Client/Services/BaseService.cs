using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NomaNova.Ojeda.Client.Results;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Client.Services
{
    internal abstract class BaseService
    {
        private static readonly JsonSerializerSettings JsonSettings;

        static BaseService()
        {
            JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false
                    }
                },
                DateParseHandling = DateParseHandling.DateTimeOffset,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            JsonSettings.Converters.Add(new StringEnumConverter());
        }

        private readonly OjedaHttpClient _httpClient;

        protected BaseService(OjedaHttpClient httpClient)
        {
            _httpClient = httpClient;
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

        private static async Task<OjedaResult> ParseResponseAsync(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                var error = await TryParseErrorAsync(response);
                return OjedaResult.ForFailure(statusCode, error);
            }

            return OjedaResult.ForSuccess(statusCode);
        }

        private static async Task<OjedaDataResult<T>> ParseDataResponseAsync<T>(HttpResponseMessage response)
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

        private static async Task<T> GetPayloadAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content, JsonSettings);
        }

        private static async Task<ErrorDto> TryParseErrorAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ErrorDto>(content, JsonSettings);
        }

        private static void TryAddPayload(HttpRequestMessage request, object payload)
        {
            if (payload == null)
            {
                return;
            }

            var body = JsonConvert.SerializeObject(payload, JsonSettings);
            request.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}