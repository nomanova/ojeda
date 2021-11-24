using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Shared.Validation
{
    public class NamedFieldValidator : AbstractValidator<INamedDto>
    {
        public NamedFieldValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty();
            RuleFor(_ =>  _.Name).MaximumLength(Constants.NameMaxLength);
        }
    }
}