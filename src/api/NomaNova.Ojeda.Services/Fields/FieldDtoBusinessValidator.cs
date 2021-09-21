using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldDtoBusinessValidator : AbstractValidator<UpsertFieldDto>
    {
        public FieldDtoBusinessValidator(IRepository<Field> fieldsRepository, string id = null)
        {
            // Field validation
            Include(new UpsertFieldDtoValidator());
            
            // Business rule: ensure unique name
            RuleFor(_ => _.Name).MustAsync(async (name, cancellation) =>
            {
                var field = (await fieldsRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(name));
                }, cancellation)).FirstOrDefault();

                if (field == null)
                {
                    return true;
                }

                return id != null && id.Equals(field.Id);

            }).OverridePropertyName(dto => dto.Name).WithMessage(dto => $"The name '{dto.Name}' is already in use.");
        }
    }
}