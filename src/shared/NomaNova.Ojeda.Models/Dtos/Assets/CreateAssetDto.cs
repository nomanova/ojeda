using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;
using NomaNova.Ojeda.Models.Dtos.Assets.Validation;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class CreateAssetDto : UpsertAssetDto<CreateAssetFieldSetDto, CreateAssetFieldDto>
    {
    }

    public class CreateAssetFieldSetDto : UpsertAssetFieldSetDto<CreateAssetFieldDto>
    {
    }

    public class CreateAssetFieldDto : UpsertAssetFieldDto
    {
    }
    
    public class CreateAssetDtoFieldValidator : CompositeValidator<CreateAssetDto>
    {
        public CreateAssetDtoFieldValidator(AssetIdTypeDto assetIdType, IFieldPropertiesResolver fieldPropertiesResolver)
        {
            RegisterBaseValidator(new AssetIdentityFieldValidator(assetIdType.Properties));
            Include(new UpsertAssetDtoFieldValidator<CreateAssetFieldSetDto, CreateAssetFieldDto>(fieldPropertiesResolver));
        }
    }
}