using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class DryRunUpdateAssetTypeDto
    {
        public List<NamedEntityDto> Assets { get; set; }
    }
}