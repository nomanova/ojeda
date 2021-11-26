using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class DeleteFieldSetDto
    {
        public List<AssetTypeSummaryDto> AssetTypes { get; set; }
        
        public List<AssetSummaryDto> Assets { get; set; }
    }
}