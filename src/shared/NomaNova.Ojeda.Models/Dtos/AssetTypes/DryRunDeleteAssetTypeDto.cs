using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class DryRunDeleteAssetTypeDto
    {
        public List<NamedEntityDto> Assets { get; set; }
    }
}