using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ValidationException = NomaNova.Ojeda.Core.Exceptions.ValidationException;

namespace NomaNova.Ojeda.Services
{
    public abstract class BaseService
    {
        protected static async Task Validate<T>(AbstractValidator<T> validator, T instance,
            CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(instance, cancellationToken);

            if (!result.IsValid)
            {
                var validationErrorList = result.Errors
                    .GroupBy(
                        e => e.PropertyName,
                        e => e.ErrorMessage,
                        (field, messages) => new {Field = field, Messages = messages.ToList()})
                    .ToDictionary(t => t.Field, t => t.Messages);

                throw new ValidationException(validationErrorList);
            }
        }
    }
}