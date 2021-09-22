using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets.Validators
{
    public class CreateFieldSetDtoFieldValidator : CompositeValidator<CreateFieldSetDto>
    {
        public CreateFieldSetDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
            
            RuleFor(_ => _.Fields).NotEmpty().WithMessage("At least one field is required.");

            RuleFor(_ => _.Fields).Must(fields =>
            {
                if (fields == null)
                {
                    return true;
                }

                var fieldIds = fields.Select(f => f.Id).ToList();
                return fieldIds.Count == fieldIds.Distinct().Count();
            }).WithMessage("A field must not be added more than once.");
            
            RuleForEach(_ => _.Fields).SetValidator(new CreateFieldSetFieldDtoValidator());
        }
    }

    public class CreateFieldSetFieldDtoValidator : AbstractValidator<CreateFieldSetFieldDto>
    {
        public CreateFieldSetFieldDtoValidator()
        {
            RuleFor(_ => _.Order).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.Id).NotEmpty();
        }
    }
}