using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Services.Assets.Interfaces;

namespace NomaNova.Ojeda.Services.Assets
{
    public class FieldDataValidator : IFieldDataValidator
    {
        public FieldDataValidator()
        {
        }

        public List<string> Validate(FieldDataDto data, FieldType type)
        {
            var validationMessages = new List<string>();

            // TODO: validations for specific types to be added here
            
            return validationMessages;
        }
    }
}