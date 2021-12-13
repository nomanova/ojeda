using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Services.AssetTypes.Validators
{
    public class UpdateAssetTypeDtoBusinessValidator : CompositeValidator<UpdateAssetTypeDto>
    {
        public UpdateAssetTypeDtoBusinessValidator(
            IRepository<FieldSet> fieldSetsRepository,
            IRepository<AssetType> assetTypesRepository,
            string id)
        {
            Include(new UpdateAssetTypeDtoFieldValidator());
            RegisterBaseValidator(new UniqueNameBusinessValidator<AssetType>(assetTypesRepository, id));

            RuleForEach(_ => _.FieldSets)
                .SetValidator(new UpdateAssetTypeFieldSetDtoBusinessValidator(fieldSetsRepository));
        }
        
        private sealed class UpdateAssetTypeFieldSetDtoBusinessValidator : CompositeValidator<UpdateAssetTypeFieldSetDto>
        {
            public UpdateAssetTypeFieldSetDtoBusinessValidator(IRepository<FieldSet> fieldSetsRepository)
            {
                RegisterBaseValidator(new ExistsBusinessValidator<FieldSet>(fieldSetsRepository));
            }
        }
    }
}