using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using NomaNova.Ojeda.Utils.Extensions;
using ValidationException = NomaNova.Ojeda.Core.Exceptions.ValidationException;

namespace NomaNova.Ojeda.Services
{
    public abstract class BaseService
    {
        protected static async Task ValidateAndThrowAsync<T>(AbstractValidator<T> validator, T instance,
            CancellationToken cancellationToken)
        {
            var validationErrorList = await ValidateAsync(validator, instance, cancellationToken);

            if (validationErrorList.HasElements())
            {
                throw new ValidationException(validationErrorList);
            }
        }

        protected static async Task<Dictionary<string, List<string>>> ValidateAsync<T>(AbstractValidator<T> validator,
            T instance,
            CancellationToken cancellationToken)
        {
            var validationErrorList = new Dictionary<string, List<string>>();

            if (instance == null)
            {
                validationErrorList.Add(typeof(T).Name, new List<string> { "Invalid entity." });
                return validationErrorList;
            }

            var result = await validator.ValidateAsync(instance, cancellationToken);

            if (!result.IsValid)
            {
                validationErrorList = result.Errors
                    .GroupBy(
                        e => e.PropertyName,
                        e => e.ErrorMessage,
                        (field, messages) => new { Field = field, Messages = messages.ToList() })
                    .ToDictionary(t => t.Field, t => t.Messages);
            }

            return validationErrorList;
        }
    }
}