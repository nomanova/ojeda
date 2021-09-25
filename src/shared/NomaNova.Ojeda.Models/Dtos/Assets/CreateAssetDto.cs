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
}