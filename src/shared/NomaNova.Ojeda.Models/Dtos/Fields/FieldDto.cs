using System;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public enum FieldTypeDto
    {
        Text,
        Number
    }
    
    public class FieldDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldPropertiesDto Properties { get; set; }
    }
}