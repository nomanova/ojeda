using FluentValidation;

namespace NomaNova.Ojeda.Models.Fields
{
    public class CreateFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public FieldTypeDto Type { get; set; }
    }
    
    public class CreateFieldDtoValidator : AbstractValidator<CreateFieldDto>
    {
        public CreateFieldDtoValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
            RuleFor(dto =>  dto.Description).NotEmpty();
        }
    }
}