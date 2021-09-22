using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets.Base
{
    public abstract class UpsertFieldSetDto<T> : INamedDto where T : UpsertFieldSetFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<T> Fields { get; set; }
    }

    public abstract class UpsertFieldSetFieldDto : IIdentityDto
    {
        public string Id { get; set; }
        
        public int Order { get; set; }
    }
}