using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Assets;

namespace NomaNova.Ojeda.Services.Assets
{
    internal class AssetDtoBusinessValidator : AbstractValidator<AssetDto>
    {
        public AssetDtoBusinessValidator(
            IRepository<Field> fieldsRepository,
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetClass> assetClassesRepository)
        {
            // Business rule: ensure asset class exists
            RuleFor(_ => _.AssetClass.Id).MustAsync(async (id, cancellation) =>
            {
                var assetClass = await assetClassesRepository.GetByIdAsync(id, cancellation);
                return assetClass != null;
            }).WithMessage("Asset class does not exist.");
            
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