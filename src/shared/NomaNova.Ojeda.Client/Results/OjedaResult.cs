using System.Net;
using NomaNova.Ojeda.Models;

namespace NomaNova.Ojeda.Client.Results
{
    public class OjedaResult
    {
        public static OjedaResult ForSuccess(HttpStatusCode statusCode)
        {
            return new OjedaResult
            {
                Success = true,
                StatusCode = statusCode,
                Error = null
            };
        }

        public static OjedaResult ForFailure(HttpStatusCode statusCode, ErrorDto error = null)
        {
            return new OjedaResult
            {
                Success = false,
                StatusCode = statusCode,
                Error = error
            };
        }

        public bool Success { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public ErrorDto Error { get; set; }
    }
}