using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using static FluentValidation.AssemblyScanner;

namespace NomaNova.Ojeda.Web.Shared.Validation
{
    public static class EditContextFluentValidationExtensions
    {
        private static readonly List<string> ScannedAssembly = new();
        private static readonly List<AssemblyScanResult> AssemblyScanResults = new();

        public static EditContext AddFluentValidation(this EditContext editContext, IServiceProvider serviceProvider,
            bool disableAssemblyScanning, IValidator validator, FluentValidationValidator fluentValidationValidator)
        {
            if (editContext == null)
            {
                throw new ArgumentNullException(nameof(editContext));
            }

            var messages = new ValidationMessageStore(editContext);

            editContext.OnValidationRequested +=
                (sender, eventArgs) => ValidateModel((EditContext) sender, messages, serviceProvider,
                    disableAssemblyScanning, fluentValidationValidator, validator);

            editContext.OnFieldChanged +=
                (sender, eventArgs) => ValidateField(editContext, messages, eventArgs.FieldIdentifier, serviceProvider,
                    disableAssemblyScanning, validator);

            return editContext;
        }

        private static async void ValidateModel(EditContext editContext,
            ValidationMessageStore messages,
            IServiceProvider serviceProvider,
            bool disableAssemblyScanning,
            FluentValidationValidator fluentValidationValidator,
            IValidator validator = null)
        {
            validator ??= GetValidatorForModel(serviceProvider, editContext.Model, disableAssemblyScanning);

            if (validator != null)
            {
                var context = ValidationContext<object>.CreateWithOptions(editContext.Model,
                    fluentValidationValidator.Options ?? (opt => opt.IncludeAllRuleSets()));

                var validationResults = await validator.ValidateAsync(context);

                messages.Clear();
                foreach (var validationResult in validationResults.Errors)
                {
                    var fieldIdentifier = FluentValidationUtils.ToFieldIdentifier(editContext, validationResult.PropertyName);
                    messages.Add(fieldIdentifier, validationResult.ErrorMessage);
                }

                editContext.NotifyValidationStateChanged();
            }
        }

        private static async void ValidateField(EditContext editContext,
            ValidationMessageStore messages,
            FieldIdentifier fieldIdentifier,
            IServiceProvider serviceProvider,
            bool disableAssemblyScanning,
            IValidator validator = null)
        {
            Console.WriteLine($"Validating field: {fieldIdentifier.Model}");

            var model = fieldIdentifier.Model;
            
            var properties = new[] {fieldIdentifier.FieldName};
            var context = new ValidationContext<object>(model, new PropertyChain(), new MemberNameValidatorSelector(properties));

            validator ??= GetValidatorForModel(serviceProvider, model, disableAssemblyScanning);
            
            Console.WriteLine($"Model: {model}");
            Console.WriteLine($"Validator: {validator}");
            
            if (validator != null)
            {
                var validationResults = await validator.ValidateAsync(context);

                messages.Clear(fieldIdentifier);
                messages.Add(fieldIdentifier, validationResults.Errors.Select(error => error.ErrorMessage));

                editContext.NotifyValidationStateChanged();
            }
        }

        private static IValidator GetValidatorForModel(IServiceProvider serviceProvider, object model,
            bool disableAssemblyScanning)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
            
            if (serviceProvider != null)
            {
                try
                {
                    if (serviceProvider.GetService(validatorType) is IValidator validator)
                    {
                        return validator;
                    }
                }
                catch (Exception)
                {
                    // NOP
                }
            }

            if (disableAssemblyScanning)
            {
                return null;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(i => !ScannedAssembly.Contains(i.FullName)))
            {
                try
                {
                    AssemblyScanResults.AddRange(FindValidatorsInAssembly(assembly));
                }
                catch (Exception)
                {
                    // NOP
                }

                ScannedAssembly.Add(assembly.FullName);
            }

            var interfaceValidatorType = typeof(IValidator<>).MakeGenericType(model.GetType());

            var modelValidatorType = AssemblyScanResults
                .FirstOrDefault(i => interfaceValidatorType.IsAssignableFrom(i.InterfaceType))?.ValidatorType;

            if (modelValidatorType == null)
            {
                return null;
            }

            return (IValidator) ActivatorUtilities.CreateInstance(serviceProvider, modelValidatorType);
        }
    }
}