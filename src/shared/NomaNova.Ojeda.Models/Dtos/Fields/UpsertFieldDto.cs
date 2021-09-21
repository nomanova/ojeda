using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public abstract class UpsertFieldDto : INamedDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldTypeDto Type { get; set; }
    }
    
    public class UpsertFieldDtoValidator : CompositeValidator<UpsertFieldDto>
    {
        public UpsertFieldDtoValidator()
        {
            RegisterBaseValidator(new NamedValidator());
        }
    }
}