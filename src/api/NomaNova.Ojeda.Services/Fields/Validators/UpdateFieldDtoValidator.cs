using FluentValidation;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields.Validators
{
    public class UpdateFieldDtoValidator : AbstractValidator<UpdateFieldDto>
    {
        public UpdateFieldDtoValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
        }
    }
}