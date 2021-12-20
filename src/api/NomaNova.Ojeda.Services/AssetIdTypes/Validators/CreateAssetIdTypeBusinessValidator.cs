using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Services.AssetIdTypes.Validators;

public class CreateAssetIdTypeBusinessValidator : CompositeValidator<CreateAssetIdTypeDto>
{
    public CreateAssetIdTypeBusinessValidator(IRepository<AssetIdType> assetIdTypesRepository)
    {
        Include(new CreateAssetIdTypeDtoFieldValidator());
        RegisterBaseValidator(new UniqueNameBusinessValidator<AssetIdType>(assetIdTypesRepository));
    }
}