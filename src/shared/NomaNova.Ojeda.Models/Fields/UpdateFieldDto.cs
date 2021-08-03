namespace NomaNova.Ojeda.Models.Fields
{
    public class UpdateFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public FieldTypeDto Type { get; set; }
    }
}