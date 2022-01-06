using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Data.Repositories;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared.Validation;
using NomaNova.Ojeda.Services.Shared.Validation;

namespace NomaNova.Ojeda.Services.Features.AssetIdTypes.Validators;

public class UpdateAssetIdTypeBusinessValidator : CompositeValidator<UpdateAssetIdTypeDto>
{
    public UpdateAssetIdTypeBusinessValidator(IRepository<AssetIdType> assetIdTypesRepository, string id)
    {
        Include(new UpdateAssetIdTypeDtoFieldValidator());
        RegisterBaseValidator(new UniqueNameBusinessValidator<AssetIdType>(assetIdTypesRepository, id));
    }
}