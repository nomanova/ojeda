using System.Collections.Generic;
using NomaNova.Ojeda.Models.AssetClasses;
using NomaNova.Ojeda.Models.Fields;

namespace NomaNova.Ojeda.Models.Assets
{
    public class AssetSummaryDto
    {
        public string Id { get; set; }

        public AssetClassSummaryDto AssetClass { get; set; }
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