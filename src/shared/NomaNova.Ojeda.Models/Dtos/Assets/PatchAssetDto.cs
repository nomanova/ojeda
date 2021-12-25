using NomaNova.Ojeda.Models.Dtos.Assets.Validation;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class PatchAssetDto : INamedDto, IAssetIdentityDto
    {
        public string Name { get; set; }

        public string AssetId { get; set; }
    }
}