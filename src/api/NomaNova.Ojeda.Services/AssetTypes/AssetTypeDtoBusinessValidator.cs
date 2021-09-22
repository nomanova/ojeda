using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;

namespace NomaNova.Ojeda.Services.AssetTypes
{
    internal class AssetTypeDtoBusinessValidator : AbstractValidator<AssetTypeDto>
    {
        public AssetTypeDtoBusinessValidator(
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository)
        {
            // Field validation
            Include(new AssetTypeDtoValidator());
            
            // Business rule: ensure unique name
            RuleFor(_ => new {_.Name, _.Id}).MustAsync(async (_, cancellation) =>
            {
                var assetType = (await assetTypesRepository.GetAllAsync(query =>
                {
                    return query.Where(f => f.Name.Equals(_.Name));
                }, cancellation)).FirstOrDefault();

                if (assetType == null)
                {
                    return true;
                }

                return _.Id != null && _.Id.Equals(assetType.Id);

            }).OverridePropertyName(_ => _.Name).WithMessage(_ => $"The name '{_.Name}' is already in use.");
            
            // Business rule: ensure field sets exist
            RuleForEach(_ => _.FieldSets)
                .SetValidator(new AssetTypeFieldSetDtoBusinessValidator(fieldSetsRepository));
        }
        
        private class AssetTypeFieldSetDtoBusinessValidator : AbstractValidator<AssetTypeFieldSetDto>
        {
            public AssetTypeFieldSetDtoBusinessValidator(IRepository<FieldSet> fieldSetsRepository)
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