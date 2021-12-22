using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class AssetTypeSummaryDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }   
    }

    public class AssetTypeDto : AssetTypeSummaryDto
    {
        public NamedEntityDto AssetIdType { get; set; }

        public List<AssetTypeFieldSetDto> FieldSets { get; set; } = new();
    }

    public class AssetTypeFieldSetDto
    {
        public int Order { get; set; }

        public FieldSetSummaryDto FieldSet { get; set; }
    }
}