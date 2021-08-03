using System;
using System.Collections.Generic;

namespace NomaNova.Ojeda.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationError> Errors { get; }

        public ValidationException(ValidationError error)
        {
            Errors = new List<ValidationError>{error};
        }
        
        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}