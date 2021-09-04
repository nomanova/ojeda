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
            RuleFor(dto => new {dto.Name, dto.Id}).MustAsync(async (dto, cancellation) =>
            {
                var field = (await fieldsRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(dto.Name));
                }, cancellation)).FirstOrDefault();

                if (field == null)
                {
                    return true;
                }

                return dto.Id != null && dto.Id.Equals(field.Id);

            }).OverridePropertyName(dto => dto.Name).WithMessage(dto => $"'{dto.Name}' is already in use.");
        }
    }
}