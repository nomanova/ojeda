using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Services.Assets
{
    internal class AssetDtoBusinessValidator : AbstractValidator<AssetDto>
    {
        public AssetDtoBusinessValidator(
            IRepository<Field> fieldsRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository)
        {
            // Business rule: ensure asset class exists
            RuleFor(_ => _.AssetType.Id).MustAsync(async (id, cancellation) =>
            {
                var assetType = await assetTypesRepository.GetByIdAsync(id, cancellation);
                return assetType != null;
            }).WithMessage("Asset type does not exist.");
            
            // Business rule: ensure field sets and fields exist
            RuleForEach(_ => _.FieldSets)
                .SetValidator(new AssetFieldSetDtoBusinessValidator(fieldsRepository, fieldSetsRepository));
            
            // TODO Business rule: ensure no field sets are missing
        }

        private class AssetFieldSetDtoBusinessValidator : AbstractValidator<AssetFieldSetDto>
        {
            public AssetFieldSetDtoBusinessValidator(
                IRepository<Field> fieldsRepository,
                IRepository<FieldSet> fieldSetsRepository)
            {
                // Business rule: ensure field set exists
                RuleFor(_ => _.Id).MustAsync(async (id, cancellation) =>
                {
                    var fieldSet = await fieldSetsRepository.GetByIdAsync(id, cancellation);
                    return fieldSet != null;
                }).WithMessage(_ => "Field set does not exist.");
                
                // Business rule: ensure fields exist
                RuleForEach(_ => _.Fields)
                    .SetValidator(new AssetFieldDtoBusinessValidator(fieldsRepository));
                
                // TODO Business rule: ensure no fields are missing
            }

            private class AssetFieldDtoBusinessValidator : AbstractValidator<AssetFieldDto>
            {
                public AssetFieldDtoBusinessValidator(IRepository<Field> fieldsRepository)
                {
                    // Business rule: ensure field exists
                    RuleFor(_ => _.Id).MustAsync(async (id, cancellation) =>
                    {
                        var fieldSet = await fieldsRepository.GetByIdAsync(id, cancellation);
                        return fieldSet != null;
                    }).WithMessage(_ => "Field does not exist.");
                }
            }
        }
    }
}