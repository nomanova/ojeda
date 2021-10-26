using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Fields.Base
{
    public abstract class UpsertFieldDto : INamedDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldPropertiesDto Properties { get; set; }
    }
    
    public class UpsertFieldDtoFieldValidator : CompositeValidator<UpsertFieldDto>
    {
        public UpsertFieldDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
            
            RuleFor(x => x.Properties).SetInheritanceValidator(_ => {
                _.Add(new TextFieldPropertiesDtoFieldValidator());
                _.Add(new NumberFieldPropertiesDtoFieldValidator());
            });
        }
    }
}