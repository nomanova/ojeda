using System;
using System.Collections.Generic;

namespace NomaNova.Ojeda.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, List<string>> ValidationErrors { get; }

        public ValidationException(string field, List<string> messages)
        {
            ValidationErrors = new Dictionary<string, List<string>>
            {
                {field, messages}
            };
        }
        
        public ValidationException(Dictionary<string, List<string>> validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}