namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class TextFieldProperties : FieldProperties
    {
        public TextFieldProperties()
        {
            Type = FieldType.Text;
        }

        public int? MinLength { get; set; }
        
        public int? MaxLength { get; set; }
    }
}