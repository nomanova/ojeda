using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.FieldSets.Base;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class CreateFieldSetDto : UpsertFieldSetDto<CreateFieldSetFieldDto>
    {
    }
    
    public class CreateFieldSetFieldDto : UpsertFieldSetFieldDto
    {
    }

    public class CreateFieldSetDtoFieldValidator : AbstractValidator<CreateFieldSetDto>
    {
        public CreateFieldSetDtoFieldValidator()
        {
            Include(new UpsertFieldSetDtoFieldValidator<CreateFieldSetFieldDto>());
        }
    }
}