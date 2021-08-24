using FluentValidation;

namespace NomaNova.Ojeda.Models.Fields
{
    public class FieldDto
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
            RuleFor(dto =>  dto.Name).NotEmpty();
        }
    }
}