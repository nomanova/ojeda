using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class DeleteAssetTypeDto
    {
        public List<AssetSummaryDto> Assets { get; set; }
    }
}