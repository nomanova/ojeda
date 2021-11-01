using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public interface IFieldPropertiesResolver
    {
        FieldPropertiesDto Resolve(string fieldId);
    }
}