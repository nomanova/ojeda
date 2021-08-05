using System.Net;
using NomaNova.Ojeda.Models.Errors;

namespace NomaNova.Ojeda.Client.Results
{
    public class OjedaDataResult<T> : OjedaResult
    {
        public static OjedaDataResult<T> ForSuccess(T data, HttpStatusCode statusCode)
        {
            return new()
            {
                Data = data,
                Success = true,
                StatusCode = statusCode,
                Error = null
            };
        }
        
        public new static OjedaDataResult<T> ForFailure(HttpStatusCode statusCode, ErrorDto error = null)
        {
            return new()
            {
                Success = false,
                StatusCode = statusCode,
                Error = error
            };
        }
        
        public T Data { get; set; }
    }
}