using NomaNova.Ojeda.Core.Domain.Fields;

namespace NomaNova.Ojeda.Services.Assets
{
    public interface IFieldValueConverter
    {
        byte[] ToBytes(string value, FieldType fieldType);

        string FromBytes(byte[] value, FieldType fieldType);
    }
}