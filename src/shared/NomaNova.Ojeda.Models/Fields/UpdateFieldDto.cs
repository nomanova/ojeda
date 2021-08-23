using FluentValidation;

namespace NomaNova.Ojeda.Models.Fields
{
    public class UpdateFieldDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public FieldTypeDto Type { get; set; }
    }
    
    public class UpdateFieldDtoValidator : AbstractValidator<UpdateFieldDto>
    {
        public UpdateFieldDtoValidator()
        {
            RuleFor(dto =>  dto.Name).NotEmpty();
            RuleFor(dto =>  dto.Description).NotEmpty();
        }
    }
}