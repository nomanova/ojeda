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
}