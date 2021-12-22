using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

public class DryRunDeleteAssetIdTypeDto
{
    public List<NamedEntityDto> AssetTypes { get; set; }
}