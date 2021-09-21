using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class FieldSetSummaryDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
    
    public class FieldSetDto : FieldSetSummaryDto
    {
        public List<FieldSetFieldDto> Fields { get; set; } = new();
    }
    
    public class FieldSetDtoValidator : AbstractValidator<FieldSetDto>
    {
        public FieldSetDtoValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty();
            RuleFor(_ =>  _.Name).MaximumLength(Constants.NameMaxLength);
            RuleFor(_ =>  _.Description).MaximumLength(Constants.DescriptionMaxLength);
            RuleFor(_ => _.Fields).NotEmpty().WithMessage("At least one field is required.");

            RuleFor(_ => _.Fields).Must(fields =>
            {
                var fieldIds = fields.Select(f => f.Field.Id).ToList();
                return fieldIds.Count == fieldIds.Distinct().Count();
            }).WithMessage("A field must not be added more than once.");
            
            RuleForEach(_ => _.Fields).SetValidator(new FieldSetFieldDtoValidator());
        }
    }
}