using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.FieldSets.Base;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class UpdateFieldSetDto : UpsertFieldSetDto<UpdateFieldSetFieldDto>
    {
    }
    
    public class UpdateFieldSetFieldDto : UpsertFieldSetFieldDto
    {
    }
    
    public class UpdateFieldSetDtoFieldValidator : AbstractValidator<UpdateFieldSetDto>
    {
        public UpdateFieldSetDtoFieldValidator()
        {
            Include(new UpsertFieldSetDtoFieldValidator<UpdateFieldSetFieldDto>());
        }
    }
}