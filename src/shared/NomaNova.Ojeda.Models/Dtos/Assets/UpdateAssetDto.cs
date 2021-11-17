using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.Assets.Base;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class UpdateAssetDto : UpsertAssetDto<UpdateAssetFieldSetDto, UpdateAssetFieldDto>
    {
    }

    public class UpdateAssetFieldSetDto : UpsertAssetFieldSetDto<UpdateAssetFieldDto>
    {
    }

    public class UpdateAssetFieldDto : UpsertAssetFieldDto
    {
    }

    public class UpdateAssetDtoFieldValidator : AbstractValidator<UpdateAssetDto>
    {
        public UpdateAssetDtoFieldValidator(IFieldPropertiesResolver fieldPropertiesResolver)
        {
            Include(new UpsertAssetDtoFieldValidator<UpdateAssetFieldSetDto, UpdateAssetFieldDto>(fieldPropertiesResolver));
        }
    }
}