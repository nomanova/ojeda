using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Services.Assets.Interfaces
{
    public interface IFieldDataConverter
    {
        string ToStorage(FieldDataDto data, FieldProperties fieldProperties);

        FieldDataDto FromStorage(string value, FieldProperties fieldProperties);

        FieldDataTypeDto GetMatchingDataType(FieldProperties fieldProperties);
    }
}