using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.Fields
{
    public class DryRunDeleteFieldDto
    {
        public List<NamedEntityDto> FieldSets { get; set; }

        public List<NamedEntityDto> Assets { get; set; }
    }
}