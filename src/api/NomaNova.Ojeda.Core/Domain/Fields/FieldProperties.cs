namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class FieldProperties
    {
        public FieldType Type { get; set; }
    }
    
    public class TextFieldProperties : FieldProperties
    {
        public TextFieldProperties()
        {
            Type = FieldType.Text;
        }

        public int? MinLength { get; set; }
        
        public int? MaxLength { get; set; }
    }
    
    public class NumberFieldProperties : FieldProperties
    {
        public NumberFieldProperties()
        {
            Type = FieldType.Number;
        }
        
        public bool WithDecimals { get; set; }

        public double? MinValue { get; set; }
        
        public double? MaxValue { get; set; }
    }
}