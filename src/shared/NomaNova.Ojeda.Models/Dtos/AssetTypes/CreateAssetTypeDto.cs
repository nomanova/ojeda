using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetTypes.Base;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class CreateAssetTypeDto : UpsertAssetTypeDto<CreateAssetTypeFieldSetDto>
    {
    }

    public class CreateAssetTypeFieldSetDto : UpsertAssetTypeFieldSetDto
    {
    }
    
    public class CreateAssetTypeDtoFieldValidator : AbstractValidator<CreateAssetTypeDto>
    {
        public CreateAssetTypeDtoFieldValidator()
        {
            Include(new UpsertAssetTypeDtoFieldValidator<CreateAssetTypeFieldSetDto>());
        }
    }
}