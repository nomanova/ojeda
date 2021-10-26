using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components.Forms;
using NomaNova.Ojeda.Models.Dtos.Assets;
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
            Console.WriteLine("Validate model");
            
            var validator = GetValidatorForModel(editContext.Model, assetDto);
            
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

        private static async void ValidateField(EditContext editContext,
            ValidationMessageStore messages,
            AssetDto assetDto,
            FieldIdentifier fieldIdentifier)
        {
            var model = fieldIdentifier.Model;
            
            var properties = new[] {fieldIdentifier.FieldName};
            var context = new ValidationContext<object>(model, new PropertyChain(), new MemberNameValidatorSelector(properties));
            
            var validator = GetValidatorForModel(model, assetDto);
            
            var validationResults = await validator.ValidateAsync(context);
            
            messages.Clear(fieldIdentifier);
            messages.Add(fieldIdentifier, validationResults.Errors.Select(error => error.ErrorMessage));

            editContext.NotifyValidationStateChanged();
        }

        private static IValidator GetValidatorForModel(object model, AssetDto assetDto)
        {
            if (model.GetType() == typeof(CreateAssetDto))
            {
                return new CreateAssetDtoFieldValidator(assetDto);
            }

            if (model.GetType() == typeof(CreateAssetFieldDto))
            {
                return new CreateAssetFieldDtoFieldValidator(assetDto);
            }

            throw new NotImplementedException($"Missing validator for model: {model.GetType()}");
        }
    }
}