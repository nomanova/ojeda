using System;
using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class DoubleFieldDataDto : FieldDataDto
    {
        public DoubleFieldDataDto()
        {
            Type = FieldDataTypeDto.Double;
        }
        
        public double? Value { get; set; }
    }
    
    public class DoubleFieldDataDtoFieldValidator : AbstractValidator<DoubleFieldDataDto>
    {
        public DoubleFieldDataDtoFieldValidator(FieldPropertiesDto fieldProperties)
        {
            RuleFor(_ => _.Value).Must((_, value, context) =>
            {
                switch (fieldProperties.Type)
                {
                    case FieldTypeDto.Number:
                        return ValidateNumberTypeField(value, (NumberFieldPropertiesDto)fieldProperties, context);
                    default:
                        throw new NotImplementedException(nameof(fieldProperties.Type));
                        
                }
            }).WithMessage("{ValidationMessage}");
        }

        private static bool ValidateNumberTypeField(
            double? value,
            NumberFieldPropertiesDto fieldProperties,
            ValidationContext<DoubleFieldDataDto> context)
        {
            if (value == null)
            {
                // Handled by required validator
                return true;
            }
            
            var hasFailures = false;

            if (fieldProperties.MinValue.HasValue && value < fieldProperties.MinValue.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage", 
                    $"Value must be greater than or equal to {fieldProperties.MinValue.Value}.");
                hasFailures = true;
            }

            if (fieldProperties.MaxValue.HasValue && value > fieldProperties.MaxValue.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage", 
                    $"Value must be less than or equal to {fieldProperties.MaxValue.Value}.");
                hasFailures = true;
            }
            
            return !hasFailures;
        }
    }
}