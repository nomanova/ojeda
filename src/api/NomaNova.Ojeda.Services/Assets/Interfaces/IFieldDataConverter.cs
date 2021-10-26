using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Services.Assets.Interfaces
{
    public interface IFieldDataConverter
    {
        byte[] ToBytes(FieldDataDto data, FieldProperties fieldProperties);

        FieldDataDto FromBytes(byte[] value, FieldProperties fieldProperties);
    }
}