using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class FieldDataDto
    {
        public FieldTypeDto Type { get; set; }
    }

    public class TextFieldDataDto : FieldDataDto
    {
        public TextFieldDataDto()
        {
            Type = FieldTypeDto.Text;
        }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }
    }

    public class TextFieldDataDtoFieldValidator : AbstractValidator<TextFieldDataDto>
    {
        public TextFieldDataDtoFieldValidator()
        {
            RuleFor(_ => new {_.MinLength, _.MaxLength}).Must(_ =>
                    !(_.MinLength.HasValue && _.MaxLength.HasValue && _.MinLength.Value > _.MaxLength.Value))
                .WithName(nameof(TextFieldDataDto.MinLength))
                .WithMessage("Min length should not be bigger than max length.");

            RuleFor(_ => new {_.MaxLength, _.MinLength}).Must(_ =>
                    !(_.MaxLength.HasValue && _.MinLength.HasValue && _.MaxLength.Value < _.MinLength.Value))
                .WithName(nameof(TextFieldDataDto.MaxLength))
                .WithMessage("Max length should not be smaller than min length.");
        }
    }

    public class NumberFieldDataDto : FieldDataDto
    {
        public NumberFieldDataDto()
        {
            Type = FieldTypeDto.Number;
        }
        
        public bool WithDecimals { get; set; }

        public double? MinValue { get; set; }

        public double? MaxValue { get; set; }
    }

    public class NumberFieldDataDtoFieldValidator : AbstractValidator<NumberFieldDataDto>
    {
        public NumberFieldDataDtoFieldValidator()
        {
            RuleFor(_ => new {_.MinValue, _.MaxValue}).Must(_ =>
                    !(_.MinValue.HasValue && _.MaxValue.HasValue && _.MinValue.Value > _.MaxValue.Value))
                .WithName(nameof(NumberFieldDataDto.MinValue))
                .WithMessage("Min value should not be bigger than max value.");

            RuleFor(_ => new {_.MaxValue, _.MinValue}).Must(_ =>
                    !(_.MaxValue.HasValue && _.MinValue.HasValue && _.MaxValue.Value < _.MinValue.Value))
                .WithName(nameof(NumberFieldDataDto.MaxValue))
                .WithMessage("Max value should not be smaller than min value.");
        }
    }
}