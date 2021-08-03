using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using NomaNova.Ojeda.Core;
using ValidationException = NomaNova.Ojeda.Core.Exceptions.ValidationException;

namespace NomaNova.Ojeda.Services
{
    public abstract class BaseService
    {
        protected static async Task Validate<T>(AbstractValidator<T> validator, T instance, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(instance, cancellationToken);
            
            if (!result.IsValid)
            {
                var validationErrorList = result.Errors
                    .Select(validationFailure => new ValidationError
                    {
                        Code = validationFailure.ErrorCode, 
                        Message = validationFailure.ErrorMessage, 
                        Property = validationFailure.PropertyName
                    }).ToList();

                throw new ValidationException(validationErrorList);
            }
        }
    }
}