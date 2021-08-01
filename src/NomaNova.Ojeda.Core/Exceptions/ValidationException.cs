using System;
using NomaNova.Ojeda.Api.Models;

namespace NomaNova.Ojeda.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public Error Error { get; }

        public ValidationException(Error error)
        {
            Error = error;
        }
    }
}