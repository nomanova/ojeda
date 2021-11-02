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
        public DoubleFieldDataDtoFieldValidator(FieldPropertiesDto fieldProperties, bool isRequired)
        {
            RuleFor(_ => _.Value).Must((_, value, context) =>
            {
                switch (fieldProperties.Type)
                {
                    case FieldTypeDto.Number:
                        return ValidateNumberTypeField(value, (NumberFieldPropertiesDto)fieldProperties, isRequired, context);
                    default:
                        throw new NotImplementedException(nameof(fieldProperties.Type));
                        
                }
            }).WithMessage("{ValidationMessage}");
        }

        private static bool ValidateNumberTypeField(
            double? value,
            NumberFieldPropertiesDto fieldProperties,
            bool isRequired,
            ValidationContext<DoubleFieldDataDto> context)
        {
            if (value == null)
            {
                if (!isRequired)
                {
                    return true;
                }

                context.MessageFormatter.AppendArgument("ValidationMessage", "Value is required.");
                return false;
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