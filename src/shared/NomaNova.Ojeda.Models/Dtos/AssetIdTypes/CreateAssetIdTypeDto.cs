using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes.Base;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

public class CreateAssetIdTypeDto : UpsertAssetIdTypeDto
{
}

public class CreateAssetIdTypeDtoFieldValidator : AbstractValidator<CreateAssetIdTypeDto>
{
    public CreateAssetIdTypeDtoFieldValidator()
    {
        Include(new UpsertAssetIdTypeDtoFieldValidator());
    }
}
