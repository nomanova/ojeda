namespace NomaNova.Ojeda.Core.Domain.Fields
{
    public class FieldData
    {
        public FieldType Type { get; set; }
    }
    
    public class TextFieldData : FieldData
    {
        public TextFieldData()
        {
            Type = FieldType.Text;
        }

        public int? MinLength { get; set; }
        
        public int? MaxLength { get; set; }
    }
    
    public class NumberFieldData : FieldData
    {
        public NumberFieldData()
        {
            Type = FieldType.Number;
        }
        
        public bool WithDecimals { get; set; }

        public double? MinValue { get; set; }
        
        public double? MaxValue { get; set; }
    }
}