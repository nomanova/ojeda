using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Models.FieldSets
{
    public class FieldSetDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<FieldSetFieldDto> Fields { get; set; } = new();
    }
    
    public class FieldSetDtoValidator : AbstractValidator<FieldSetDto>
    {
        public FieldSetDtoValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty();
            RuleFor(_ =>  _.Name).MaximumLength(40);
            RuleFor(_ =>  _.Description).MaximumLength(250);
            RuleFor(_ => _.Fields).NotEmpty().WithMessage("At least one field is required.");

            RuleFor(_ => _.Fields).Must(fields =>
            {
                var fieldIds = fields.Select(f => f.Field.Id).ToList();
                return fieldIds.Count == fieldIds.Distinct().Count();
            }).WithMessage("A field must not be added more than once.");
            
            RuleForEach(_ => _.Fields).SetValidator(new FieldSetFieldDtoValidator());
        }
    }
    
    public class FieldSetFieldDto
    {
        public int Order { get; set; }

        public FieldDto Field { get; set; }
    }

    public class FieldSetFieldDtoValidator : AbstractValidator<FieldSetFieldDto>
    {
        public FieldSetFieldDtoValidator()
        {
            RuleFor(_ => _.Order).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.Field.Id).NotEmpty().WithMessage("Field id is missing.");
        }
    }
}