using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetTypes.Base;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class UpdateAssetTypeDto : UpsertAssetTypeDto<UpdateAssetTypeFieldSetDto>
    {
    }
    
    public class UpdateAssetTypeFieldSetDto : UpsertAssetTypeFieldSetDto
    {
    }
    
    public class UpdateAssetTypeDtoFieldValidator : AbstractValidator<UpdateAssetTypeDto>
    {
        public UpdateAssetTypeDtoFieldValidator()
        {
            Include(new UpsertAssetTypeDtoFieldValidator<UpdateAssetTypeFieldSetDto>());
        }
    }
}