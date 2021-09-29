using System;
using System.Text;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Services.Assets.Interfaces;

namespace NomaNova.Ojeda.Services.Assets
{
    public class FieldValueConverter : IFieldValueConverter
    {
        public FieldValueConverter()
        {
        }

        public byte[] ToBytes(string value, FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Text:
                    return Encoding.UTF8.GetBytes(value ?? string.Empty);
                default:
                    throw new NotImplementedException(fieldType.ToString());
            }
        }

        public string FromBytes(byte[] value, FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Text:
                    return Encoding.UTF8.GetString(value);
                default:
                    throw new NotImplementedException(fieldType.ToString());
            }
        }
    }
}