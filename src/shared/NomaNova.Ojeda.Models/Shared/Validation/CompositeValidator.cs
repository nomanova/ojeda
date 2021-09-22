using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace NomaNova.Ojeda.Models.Shared.Validation
{
    public abstract class CompositeValidator<T> : AbstractValidator<T>
    {
        private readonly List<IValidator> _otherValidators = new();

        protected void RegisterBaseValidator<TBase>(IValidator<TBase> validator)
        {
            if (validator.CanValidateInstancesOfType(typeof(T)))
            {
                _otherValidators.Add(validator);
            }
            else
            {
                throw new NotSupportedException(
                    $"Type {typeof(TBase).Name} is not a base-class or interface implemented by {typeof(T).Name}.");
            }
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var mainErrors = base.Validate(context).Errors;
            
            var errorsFromOtherValidators = _otherValidators.SelectMany(x => x.Validate(context).Errors);
            
            var combinedErrors = mainErrors.Concat(errorsFromOtherValidators).Distinct();

            return new ValidationResult(combinedErrors);
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = new ())
        {
            var mainErrors = (await base.ValidateAsync(context, cancellation)).Errors;

            var validationTasks = 
                _otherValidators.Select(otherValidator => otherValidator.ValidateAsync(context, cancellation)).ToList();

            await Task.WhenAll(validationTasks);

            var errorsFromOtherValidators = validationTasks
                .Select(_ => _.Result)
                .SelectMany(_ => _.Errors)
                .ToList();
            
            var combinedErrors = mainErrors.Concat(errorsFromOtherValidators).Distinct();
            
            return new ValidationResult(combinedErrors);
        }
    }
}