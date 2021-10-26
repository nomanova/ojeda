namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public enum FieldDataTypeDto
    {
        String,
        Long,
        Double
    }
    
    public class FieldDataDto
    {
        public FieldDataTypeDto Type { get; set; }
    }
    
    public class StringFieldDataDto : FieldDataDto
    {
        public StringFieldDataDto()
        {
            Type = FieldDataTypeDto.String;
        }
        
        public string Value { get; set; }
    }
    
    public class LongFieldDataDto : FieldDataDto
    {
        public LongFieldDataDto()
        {
            Type = FieldDataTypeDto.Long;
        }
        
        public long? Value { get; set; }
    }
    
    public class DoubleFieldDataDto : FieldDataDto
    {
        public DoubleFieldDataDto()
        {
            Type = FieldDataTypeDto.Double;
        }
        
        public double? Value { get; set; }
    }
}