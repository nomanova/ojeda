using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetDto <T, TS> 
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public string AssetTypeId { get; set; }

        public List<T> FieldSets { get; set; }
    }

    public abstract class UpsertAssetFieldSetDto<T> where T : UpsertAssetFieldDto
    {
        public string Id { get; set; }
        
        public List<T> Fields { get; set; }
    }

    public abstract class UpsertAssetFieldDto
    {
        public string Id { get; set; }

        public FieldDataDto Data { get; set; }
    }

    public class UpsertAssetDtoFieldValidator<T, TS> : AbstractValidator<UpsertAssetDto<T, TS>> 
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public UpsertAssetDtoFieldValidator(AssetDto assetDto)
        {
            RuleForEach(_ => _.FieldSets).SetValidator(new UpsertAssetFieldSetDtoFieldValidator<TS>(assetDto));
        }
    }

    public class UpsertAssetFieldSetDtoFieldValidator<T> : AbstractValidator<UpsertAssetFieldSetDto<T>>
        where T : UpsertAssetFieldDto
    {
        public UpsertAssetFieldSetDtoFieldValidator(AssetDto assetDto)
        {
            RuleForEach(_ => _.Fields).SetValidator(new UpsertAssetFieldDtoFieldValidator(assetDto));
        }
    }

    public class UpsertAssetFieldDtoFieldValidator : AbstractValidator<UpsertAssetFieldDto>
    {
        public UpsertAssetFieldDtoFieldValidator(AssetDto assetDto)
        {
            RuleFor(_ => new {_.Id, _.Data}).Must((dto, _, context) =>
            {
                var fieldId = _.Id;

                var assetField = assetDto.FieldSets
                    .SelectMany(fs => fs.Fields)
                    .FirstOrDefault(f => f.Id.Equals(fieldId));

                if (assetField == null)
                {
                    throw new ArgumentException($"Invalid state, field {fieldId} unknown.");
                }

                switch (assetField.Properties.Type)
                {
                    case FieldTypeDto.Text:
                        return ValidateTextTypeField((StringFieldDataDto) _.Data, (TextFieldPropertiesDto) assetField.Properties, context);
                    case FieldTypeDto.Number:
                        //return ValidateNumberTypeField(_.Value, (NumberFieldDataDto) assetField.Data, context);
                    default:
                        throw new NotImplementedException(nameof(assetField.Properties.Type));
                }
            }).OverridePropertyName(_ => _.Data).WithMessage("{ValidationMessage}");
        }

        private static bool ValidateTextTypeField(StringFieldDataDto data, TextFieldPropertiesDto fieldProperties, ValidationContext<UpsertAssetFieldDto> context)
        {
            if (data.Value == null)
            {
                // Handled by required validator
                return true;
            }

            var hasFailures = false;
            
            if (fieldProperties.MinLength.HasValue && data.Value.Length < fieldProperties.MinLength.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage",$"The length must be at least {fieldProperties.MinLength.Value} characters. You entered {data.Value.Length} character(s).");
                hasFailures = true;
            }

            if (fieldProperties.MaxLength.HasValue && data.Value.Length > fieldProperties.MaxLength.Value)
            {
                context.MessageFormatter.AppendArgument("ValidationMessage",$"The length must be {fieldProperties.MaxLength.Value} characters or fewer. You entered {data.Value.Length} character(s).");
                hasFailures = true;
            }

            return !hasFailures;
        }

        // private static bool ValidateNumberTypeField( value, NumberFieldDataDto fieldData, ValidationContext<UpsertAssetFieldDto> context)
        // {
        //     Console.WriteLine("Validating number type");
        //     return true;
        // }
    }
}