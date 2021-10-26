using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class FieldPropertiesDto
    {
        public FieldTypeDto Type { get; set; }
    }

    public class TextFieldPropertiesDto : FieldPropertiesDto
    {
        public TextFieldPropertiesDto()
        {
            Type = FieldTypeDto.Text;
        }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }
    }

    public class TextFieldPropertiesDtoFieldValidator : AbstractValidator<TextFieldPropertiesDto>
    {
        public TextFieldPropertiesDtoFieldValidator()
        {
            RuleFor(_ => _.MinLength).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.MaxLength).GreaterThanOrEqualTo(0);
            
            RuleFor(_ => new {_.MinLength, _.MaxLength}).Must(_ =>
                    !(_.MinLength.HasValue && _.MaxLength.HasValue && _.MinLength.Value > _.MaxLength.Value))
                .WithName(nameof(TextFieldPropertiesDto.MinLength))
                .WithMessage("'Min length' should not be bigger than 'Max Length'.");

            RuleFor(_ => new {_.MaxLength, _.MinLength}).Must(_ =>
                    !(_.MaxLength.HasValue && _.MinLength.HasValue && _.MaxLength.Value < _.MinLength.Value))
                .WithName(nameof(TextFieldPropertiesDto.MaxLength))
                .WithMessage("'Max Length' should not be smaller than 'Min Length'.");
        }
    }

    public class NumberFieldPropertiesDto : FieldPropertiesDto
    {
        public NumberFieldPropertiesDto()
        {
            Type = FieldTypeDto.Number;
        }
        
        public bool WithDecimals { get; set; }

        public double? MinValue { get; set; }

        public double? MaxValue { get; set; }
    }

    public class NumberFieldPropertiesDtoFieldValidator : AbstractValidator<NumberFieldPropertiesDto>
    {
        public NumberFieldPropertiesDtoFieldValidator()
        {
            RuleFor(_ => new {_.MinValue, _.MaxValue}).Must(_ =>
                    !(_.MinValue.HasValue && _.MaxValue.HasValue && _.MinValue.Value > _.MaxValue.Value))
                .WithName(nameof(NumberFieldPropertiesDto.MinValue))
                .WithMessage("'Min Value' should not be bigger than 'Max Value'.");

            RuleFor(_ => new {_.MaxValue, _.MinValue}).Must(_ =>
                    !(_.MaxValue.HasValue && _.MinValue.HasValue && _.MaxValue.Value < _.MinValue.Value))
                .WithName(nameof(NumberFieldPropertiesDto.MaxValue))
                .WithMessage("'Max Value' should not be smaller than 'Min Value'.");
        }
    }
}