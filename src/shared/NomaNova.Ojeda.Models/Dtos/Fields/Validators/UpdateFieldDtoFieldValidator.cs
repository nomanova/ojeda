using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Fields.Validators
{
    public class UpdateFieldDtoFieldValidator : CompositeValidator<UpdateFieldDto>
    {
        public UpdateFieldDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
        }
    }
}