using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields.Base;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class CreateFieldDto : UpsertFieldDto
    {
    }
    
    public class CreateFieldDtoFieldValidator : AbstractValidator<CreateFieldDto>
    {
        public CreateFieldDtoFieldValidator()
        {
            Include(new UpsertFieldDtoFieldValidator());
        }
    }
}