using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared.Validation;
using NomaNova.Ojeda.Services.Shared.Validation;

namespace NomaNova.Ojeda.Services.Features.Fields.Validators;

public class CreateFieldDtoBusinessValidator : CompositeValidator<CreateFieldDto>
{
    public CreateFieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
    {
        Include(new CreateFieldDtoFieldValidator());
        RegisterBaseValidator(new UniqueNameBusinessValidator<Field>(fieldsRepository));
    }
}