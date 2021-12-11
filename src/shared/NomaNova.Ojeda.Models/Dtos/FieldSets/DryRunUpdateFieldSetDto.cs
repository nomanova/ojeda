using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.FieldSets
{
    public class DryRunUpdateFieldSetDto
    {
        public List<NamedEntityDto> Assets { get; set; }
    }
}