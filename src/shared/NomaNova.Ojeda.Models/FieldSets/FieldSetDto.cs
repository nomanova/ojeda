using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.FieldSets
{
    public class FieldSetDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<FieldSetFieldDto> Fields { get; set; }
    }
}