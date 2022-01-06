using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Services.Shared.Validation;

public class UniqueNameBusinessValidator<T> : AbstractValidator<INamedDto> where T : BaseEntity, INamedEntity
{
    public UniqueNameBusinessValidator(IRepository<T> repository, string id = null)
    {
        RuleFor(_ => _.Name).MustAsync(async (name, cancellation) =>
        {
            var namedEntity = (await repository.GetAllAsync(query =>
            {
                return query.Where(f => f.Name.Equals(name));
            }, cancellation)).FirstOrDefault();

            if (namedEntity == null)
            {
                return true;
            }

            return id != null && id.Equals(namedEntity.Id);

        }).OverridePropertyName(dto => dto.Name).WithMessage(dto => $"The name '{dto.Name}' is already in use.");
    }
}