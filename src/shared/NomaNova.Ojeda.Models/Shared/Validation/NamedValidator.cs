using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Shared.Validation
{
    public class NamedValidator : AbstractValidator<INamedDto>
    {
        public NamedValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty();
            RuleFor(_ =>  _.Name).MaximumLength(Constants.NameMaxLength);
            RuleFor(_ =>  _.Description).MaximumLength(Constants.DescriptionMaxLength);
        }
    }
}