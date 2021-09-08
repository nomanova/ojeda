using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public class FieldSetDtoBusinessValidator : AbstractValidator<FieldSetDto>
    {
        public FieldSetDtoBusinessValidator(
            IRepository<Field> fieldsRepository, 
            IRepository<FieldSet> fieldSetsRepository)
        {
            // Field validation
            Include(new FieldSetDtoValidator());
            
            // Business rule: ensure unique name
            RuleFor(_ => new {_.Name, _.Id}).MustAsync(async (_, cancellation) =>
            {
                var fieldSet = (await fieldSetsRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(_.Name));
                }, cancellation)).FirstOrDefault();

                if (fieldSet == null)
                {
                    return true;
                }

                return _.Id != null && _.Id.Equals(fieldSet.Id);

            }).OverridePropertyName(_ => _.Name).WithMessage(_ => $"The name '{_.Name}' is already in use.");
            
            // Business rule: ensure fields exist
            RuleForEach(_ => _.Fields)
                .SetValidator(new FieldSetFieldDtoBusinessValidator(fieldsRepository));
        }
        
        private class FieldSetFieldDtoBusinessValidator : AbstractValidator<FieldSetFieldDto>
        {
            public FieldSetFieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
            {
                RuleFor(_ => _.Field.Id).MustAsync(async (id, cancellation) =>
                {
                    var field = await fieldsRepository.GetByIdAsync(id, cancellation);
                    return field != null;
                }).WithMessage("Field does not exist.");
            }
        }
    }
}