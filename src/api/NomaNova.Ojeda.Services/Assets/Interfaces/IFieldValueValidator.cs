using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Services.Assets.Interfaces
{
    public interface IFieldValueValidator
    {
        List<string> Validate(string value, FieldType type);
    }
}