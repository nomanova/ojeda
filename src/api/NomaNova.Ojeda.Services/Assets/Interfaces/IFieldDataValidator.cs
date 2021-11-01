using System.Collections.Generic;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Services.Assets.Interfaces
{
    public interface IFieldDataValidator
    {
        List<string> Validate(FieldDataDto data, FieldProperties properties);
    }
}