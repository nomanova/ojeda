using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Shared.Validation
{
    public class DescribedFieldValidator : AbstractValidator<IDescribedDto>
    {
        public DescribedFieldValidator()
        {
            RuleFor(_ =>  _.Description).MaximumLength(Constants.DescriptionMaxLength);
        }
    }
}