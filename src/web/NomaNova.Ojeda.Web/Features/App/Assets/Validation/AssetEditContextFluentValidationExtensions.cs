using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components.Forms;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;
using NomaNova.Ojeda.Web.Shared.Validation;

namespace NomaNova.Ojeda.Web.Features.App.Assets.Validation
{
    public static class AssetEditContextFluentValidationExtensions
    {
        public static void AddFluentValidation(this EditContext editContext, AssetDto assetDto)
        {
            if (editContext == null)
            {
                throw new ArgumentNullException(nameof(editContext));
            }
            
            var messages = new ValidationMessageStore(editContext);
            
            editContext.OnValidationRequested +=
                (sender, _) => ValidateModel((EditContext) sender, messages, assetDto);
            
            editContext.OnFieldChanged +=
                (_, eventArgs) => ValidateField(editContext, messages, assetDto, eventArgs.FieldIdentifier);
        }

        private static async void ValidateModel(
            EditContext editContext,
            ValidationMessageStore messages,
            AssetDto assetDto)
        {
            var validator = (IValidator) new CreateAssetDtoFieldValidator(new AssetDtoFieldPropertiesResolver(assetDto));

            var context = ValidationContext<object>.CreateWithOptions(editContext.Model,
                opt => opt.IncludeAllRuleSets());
            
            var validationResults = await validator.ValidateAsync(context);
            
            messages.Clear();
            
            foreach (var validationResult in validationResults.Errors)
            {
                var fieldIdentifier = FluentValidationUtils.ToFieldIdentifier(editContext, validationResult.PropertyName);
                messages.Add(fieldIdentifier, validationResult.ErrorMessage);
            }

            editContext.NotifyValidationStateChanged();
        }

        private static async void ValidateField(
            EditContext editContext,
            ValidationMessageStore messages,
            AssetDto assetDto,
            FieldIdentifier fieldIdentifier)
        {
            var model = fieldIdentifier.Model;
            var fieldId = GetFieldId(editContext.Model, model);

            var properties = new[] {fieldIdentifier.FieldName};
            var context = new ValidationContext<object>(model, new PropertyChain(), new MemberNameValidatorSelector(properties));
            
            var validator = GetFieldDataValidator(model, assetDto, fieldId);
            
            var validationResults = await validator.ValidateAsync(context);
            
            messages.Clear(fieldIdentifier);
            messages.Add(fieldIdentifier, validationResults.Errors.Select(error => error.ErrorMessage));

            editContext.NotifyValidationStateChanged();
        }

        private static IValidator GetFieldDataValidator(object model, AssetDto assetDto, string fieldId)
        {
            var resolver = new AssetDtoFieldPropertiesResolver(assetDto);
            var (fieldProperties, isRequired) = resolver.Resolve(fieldId);
            
            if (model.GetType() == typeof(StringFieldDataDto))
            {
                return new StringFieldDataDtoFieldValidator(fieldProperties, isRequired);
            }

            if (model.GetType() == typeof(LongFieldDataDto))
            {
                return new LongFieldDataDtoFieldValidator(fieldProperties, isRequired);
            }
            
            if (model.GetType() == typeof(DoubleFieldDataDto))
            {
                return new DoubleFieldDataDtoFieldValidator(fieldProperties, isRequired);
            }

            throw new NotImplementedException($"Missing validator for model: {model.GetType()}");
        }

        private static string GetFieldId(object editContextModel, object fieldModel)
        {
            var createAssetDto = (CreateAssetDto)editContextModel;

            foreach (var createAssetFieldSetDto in createAssetDto.FieldSets)
            {
                foreach (var createAssetFieldDto in createAssetFieldSetDto.Fields)
                {
                    if (createAssetFieldDto.Data.Equals(fieldModel))
                    {
                        return createAssetFieldDto.Id;
                    }
                }
            }

            throw new ArgumentException("Invalid state, no matching field found");
        }
    }
}