using System;
using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class StringFieldDataDto : FieldDataDto
    {
        public StringFieldDataDto()
        {
            Type = FieldDataTypeDto.String;
        }
        
        public string Value { get; set; }
    }

    public class StringFieldDataDtoFieldValidator : AbstractValidator<StringFieldDataDto>
    {
        public StringFieldDataDtoFieldValidator(FieldPropertiesDto fieldProperties)
        {
            RuleFor(_ => _.Value).Must((_, value, context) =>
            {
                switch (fieldProperties.Type)
                {
                    case FieldTypeDto.Text:
                        return ValidateTextTypeField(value, (TextFieldPropertiesDto)fieldProperties, context);
                    default:
                        throw new NotImplementedException(nameof(fieldProperties.Type));
                }
            }).WithMessage("{ValidationMessage}");
        }
        
        private static bool ValidateTextTypeField(
            string value, 
            TextFieldPropertiesDto fieldProperties,
            ValidationContext<StringFieldDataDto> context)
        {
            if (value == null)
            {
                // Handled by required validator
                return true;
            }
        
            var hasFailures = false;
        
            if (fieldProperties.MinLength.HasValue && value.Length < fieldProperties.MinLength.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage",
                    $"The length must be at least {fieldProperties.MinLength.Value} character(s). You entered {value.Length} character(s).");
                hasFailures = true;
            }
        
            if (fieldProperties.MaxLength.HasValue && value.Length > fieldProperties.MaxLength.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage",
                    $"The length must be {fieldProperties.MaxLength.Value} character(s) or fewer. You entered {value.Length} character(s).");
                hasFailures = true;
            }
        
            return !hasFailures;
        }
    }
}