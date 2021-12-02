using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.Assets;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class DryRunUpdateFieldSetDto
    {
        public List<AssetSummaryDto> Assets { get; set; }
    }
}