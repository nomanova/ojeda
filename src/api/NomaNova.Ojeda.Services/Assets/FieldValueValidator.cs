using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Services.Assets.Interfaces;

namespace NomaNova.Ojeda.Services.Assets
{
    public class FieldValueValidator : IFieldValueValidator
    {
        public FieldValueValidator()
        {
        }

        public List<string> Validate(string value, FieldType type)
        {
            var validationMessages = new List<string>();

            // TODO: validations for specific types to be added here
            
            return validationMessages;
        }
    }
}