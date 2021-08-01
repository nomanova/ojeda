namespace NomaNova.Ojeda.Models.Fields
{
    public class CreateFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public FieldTypeDto FieldType { get; set; }
    }
}