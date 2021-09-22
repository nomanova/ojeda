using FluentValidation;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Services
{
    public class ExistsBusinessValidator<T> : AbstractValidator<IIdentityDto> where T : BaseEntity
    {
        public ExistsBusinessValidator(IRepository<T> repository)
        {
            RuleFor(_ => _.Id).MustAsync(async (id, cancellation) =>
            {
                var entity = await repository.GetByIdAsync(id, cancellation);
                return entity != null;
            }).WithMessage("The item does not exist.");
        }
    }
}