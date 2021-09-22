using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Fields.Validators
{
    public class CreateFieldDtoFieldValidator : CompositeValidator<CreateFieldDto>
    {
        public CreateFieldDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
        }
    }
}