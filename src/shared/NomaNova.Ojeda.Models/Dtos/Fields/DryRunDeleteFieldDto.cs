using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Dtos.FieldSets;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class DryRunDeleteFieldDto
    {
        public List<FieldSetSummaryDto> FieldSets { get; set; }

        public List<AssetSummaryDto> Assets { get; set; }
    }
}