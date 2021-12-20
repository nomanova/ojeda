using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes.Base;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

public class UpdateAssetIdTypeDto : UpsertAssetIdTypeDto
{
}

public class UpdateAssetIdTypeDtoFieldValidator : AbstractValidator<UpdateAssetIdTypeDto>
{
    public UpdateAssetIdTypeDtoFieldValidator()
    {
        Include(new UpsertAssetIdTypeDtoFieldValidator());
    }
}