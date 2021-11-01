namespace NomaNova.Ojeda.Core.Domain.Fields
{
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