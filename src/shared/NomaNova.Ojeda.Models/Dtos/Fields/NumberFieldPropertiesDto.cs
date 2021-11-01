using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
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