using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class CreateFieldSetDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<CreateFieldSetFieldDto> Fields { get; set; }
    }

    public class CreateFieldSetFieldDto
    {
        public string Id { get; set; }
        
        public int Order { get; set; }
    }
}