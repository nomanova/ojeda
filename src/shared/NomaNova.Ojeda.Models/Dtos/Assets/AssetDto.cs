using System;
using System.Collections.Generic;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.Assets
{
    public class AssetSummaryDto : IIdentityDto
    {
        public string Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public AssetTypeSummaryDto AssetType { get; set; }
    }

    public class AssetDto : AssetSummaryDto
    {
        public List<AssetFieldSetDto> FieldSets { get; set; } = new();
    }

    public class AssetFieldSetDto
    {
        public string Id { get; set; }

        public uint Order { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<AssetFieldDto> Fields { get; set; } = new();
    }

    public class AssetFieldDto
    {
        public string Id { get; set; }

        public uint Order { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public FieldTypeDto Type { get; set; }

        public string Value { get; set; }
    }
}