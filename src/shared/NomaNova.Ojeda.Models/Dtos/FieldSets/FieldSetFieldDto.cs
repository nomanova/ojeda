using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
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