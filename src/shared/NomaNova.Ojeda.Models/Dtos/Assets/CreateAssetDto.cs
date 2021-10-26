using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;

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
    
    public class CreateAssetDtoFieldValidator : AbstractValidator<CreateAssetDto>
    {
        public CreateAssetDtoFieldValidator(AssetDto assetDto)
        {
            Include(new UpsertAssetDtoFieldValidator<CreateAssetFieldSetDto, CreateAssetFieldDto>(assetDto));
        }
    }

    public class CreateAssetFieldDtoFieldValidator : AbstractValidator<CreateAssetFieldDto>
    {
        public CreateAssetFieldDtoFieldValidator(AssetDto assetDto)
        {
            Include(new UpsertAssetFieldDtoFieldValidator(assetDto));
        }
    }
}