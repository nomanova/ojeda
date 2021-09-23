using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields.Base;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class UpdateFieldDto : UpsertFieldDto
    {
    }
    
    public class UpdateFieldDtoFieldValidator : AbstractValidator<UpdateFieldDto>
    {
        public UpdateFieldDtoFieldValidator()
        {
            Include(new UpsertFieldDtoFieldValidator());
        }
    }
}