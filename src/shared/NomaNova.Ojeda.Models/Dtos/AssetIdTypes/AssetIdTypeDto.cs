using System;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes
{
    public enum SymbologyDto
    {
        Ean13
    }

    public class AssetIdTypeSummaryDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool WithManualEntry { get; set; }
    }

    public class AssetIdTypeDto : AssetIdTypeSummaryDto
    {
        public SymbologyPropertiesDto Properties { get; set; }
    }
}