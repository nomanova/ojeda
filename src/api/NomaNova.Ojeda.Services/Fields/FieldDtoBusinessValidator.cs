using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldDtoBusinessValidator : AbstractValidator<FieldDto>
    {
        public FieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
        {
            // Field validation
            Include(new FieldDtoValidator());
            
            // Business rule: ensure unique name
            RuleFor(_ => new {_.Name, _.Id}).MustAsync(async (_, cancellation) =>
            {
                var field = (await fieldsRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(_.Name));
                }, cancellation)).FirstOrDefault();

                if (field == null)
                {
                    return true;
                }

                return _.Id != null && _.Id.Equals(field.Id);

            }).OverridePropertyName(dto => dto.Name).WithMessage(dto => $"The name '{dto.Name}' is already in use.");
        }
    }
}