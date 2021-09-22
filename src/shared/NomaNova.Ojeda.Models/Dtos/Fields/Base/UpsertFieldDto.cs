using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.Fields.Base
{
    public abstract class UpsertFieldDto : INamedDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldTypeDto Type { get; set; }
    }
}