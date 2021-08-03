using FluentValidation;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields.Validators
{
    public class CreateFieldDtoValidator : AbstractValidator<CreateFieldDto>
    {
        public CreateFieldDtoValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
        }
    }
}