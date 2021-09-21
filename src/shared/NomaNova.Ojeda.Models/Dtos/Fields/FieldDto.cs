using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class FieldDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldTypeDto Type { get; set; }
    }
}