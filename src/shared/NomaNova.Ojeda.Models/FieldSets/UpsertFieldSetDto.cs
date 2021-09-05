using System.Collections.Generic;
using FluentValidation;

namespace NomaNova.Ojeda.Models.FieldSets
{
    public class UpsertFieldSetDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<UpsertFieldSetFieldDto> Fields { get; set; }
    }
    
    public class UpsertFieldSetDtoFieldValidator : AbstractValidator<UpsertFieldSetDto>
    {
        public UpsertFieldSetDtoFieldValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
        }
    }
}