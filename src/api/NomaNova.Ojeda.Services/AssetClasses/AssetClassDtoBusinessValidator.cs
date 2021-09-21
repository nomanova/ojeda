using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.AssetClasses;

namespace NomaNova.Ojeda.Services.AssetClasses
{
    internal class AssetClassDtoBusinessValidator : AbstractValidator<AssetClassDto>
    {
        public AssetClassDtoBusinessValidator(
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetClass> assetClassesRepository)
        {
            // Field validation
            Include(new AssetClassDtoValidator());
            
            // Business rule: ensure unique name
            RuleFor(_ => new {_.Name, _.Id}).MustAsync(async (_, cancellation) =>
            {
                var assetClass = (await assetClassesRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(_.Name));
                }, cancellation)).FirstOrDefault();

                if (assetClass == null)
                {
                    return true;
                }

                return _.Id != null && _.Id.Equals(assetClass.Id);

            }).OverridePropertyName(_ => _.Name).WithMessage(_ => $"The name '{_.Name}' is already in use.");
            
            // Business rule: ensure field sets exist
            RuleForEach(_ => _.FieldSets)
                .SetValidator(new AssetClassFieldSetDtoBusinessValidator(fieldSetsRepository));
        }
        
        private class AssetClassFieldSetDtoBusinessValidator : AbstractValidator<AssetClassFieldSetDto>
        {
            public AssetClassFieldSetDtoBusinessValidator(IRepository<FieldSet> fieldSetsRepository)
            {
                // Business rule: ensure field set exists
                RuleFor(_ => _.FieldSet.Id).MustAsync(async (id, cancellation) =>
                {
                    var fieldSet = await fieldSetsRepository.GetByIdAsync(id, cancellation);
                    return fieldSet != null;
                }).WithMessage("Field set does not exist.");
            }
        }
    }
}