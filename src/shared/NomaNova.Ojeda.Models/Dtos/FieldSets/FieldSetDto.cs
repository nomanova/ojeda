using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class FieldSetSummaryDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
    
    public class FieldSetDto : FieldSetSummaryDto
    {
        public List<FieldSetFieldDto> Fields { get; set; } = new();
    }
    
    public class FieldSetFieldDto
    {
        public int Order { get; set; }

        public FieldDto Field { get; set; }

        public bool IsRequired { get; set; }
    }
}