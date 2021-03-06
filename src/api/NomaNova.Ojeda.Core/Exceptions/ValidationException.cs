using System;
using System.Collections.Generic;

namespace NomaNova.Ojeda.Core.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public Dictionary<string, List<string>> ValidationErrors { get; }

        public ValidationException(string field, string message)
        {
            ValidationErrors = new Dictionary<string, List<string>>
            {
                {field, new List<string> {message}}
            };
        }

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