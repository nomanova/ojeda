using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class DryRunDeleteFieldSetDto
    {
        public List<NamedEntityDto> AssetTypes { get; set; }
        
        public List<NamedEntityDto> Assets { get; set; }
    }
}