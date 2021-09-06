using System.Collections.Generic;
using FluentValidation;

namespace NomaNova.Ojeda.Models.FieldSets
{
    public class FieldSetDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<FieldSetFieldDto> Fields { get; set; } = new();
    }
    
    public class FieldSetDtoFieldValidator : AbstractValidator<FieldSetDto>
    {
        public FieldSetDtoFieldValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
        }
    }
}