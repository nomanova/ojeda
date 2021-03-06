using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared.Validation;
using NomaNova.Ojeda.Services.Shared.Validation;

namespace NomaNova.Ojeda.Services.Features.Fields.Validators;

public class UpdateFieldDtoBusinessValidator : CompositeValidator<UpdateFieldDto>
{
    public UpdateFieldDtoBusinessValidator(IRepository<Field> fieldsRepository, string id)
    {
        Include(new UpdateFieldDtoFieldValidator());
        RegisterBaseValidator(new UniqueNameBusinessValidator<Field>(fieldsRepository, id));
    }
}