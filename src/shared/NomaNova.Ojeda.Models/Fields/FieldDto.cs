using FluentValidation;

namespace NomaNova.Ojeda.Models.Fields
{
    public enum FieldTypeDto
    {
        Text
    }
    
    public class FieldDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldTypeDto Type { get; set; }
    }
    
    public class FieldDtoValidator : AbstractValidator<FieldDto>
    {
        public FieldDtoValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty().WithMessage("Please provide a name.");
            RuleFor(_ =>  _.Name).MaximumLength(40);
            RuleFor(_ =>  _.Description).MaximumLength(250);
        }
    }
}