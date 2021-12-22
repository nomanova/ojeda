using FluentValidation;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Services.AssetTypes.Validators
{
    internal class CreateAssetTypeDtoBusinessValidator : CompositeValidator<CreateAssetTypeDto>
    {
        public CreateAssetTypeDtoBusinessValidator(
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository,
            IRepository<AssetIdType> assetIdTypesRepository)
        {
            Include(new CreateAssetTypeDtoFieldValidator());
            RegisterBaseValidator(new UniqueNameBusinessValidator<AssetType>(assetTypesRepository));

            RuleFor(_ => _.AssetIdTypeId).MustAsync(async (id, cancellation) =>
                {
                    var entity = await assetIdTypesRepository.GetByIdAsync(id, cancellation);
                    return entity != null;
                }).WithMessage("The 'Id Type' does not exist.");

            RuleForEach(_ => _.FieldSets)
                .SetValidator(new CreateAssetTypeFieldSetDtoBusinessValidator(fieldSetsRepository));
        }
        
        private sealed class CreateAssetTypeFieldSetDtoBusinessValidator : CompositeValidator<CreateAssetTypeFieldSetDto>
        {
            public CreateAssetTypeFieldSetDtoBusinessValidator(IRepository<FieldSet> fieldSetsRepository)
            {
                RegisterBaseValidator(new ExistsBusinessValidator<FieldSet>(fieldSetsRepository));
            }
        }
    }
}