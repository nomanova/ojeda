using System;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using NomaNova.Ojeda.Web.Features.App.Assets.Services.Interfaces;

namespace NomaNova.Ojeda.Web.Features.App.Assets.Services;

public class SymbologyService : ISymbologyService
{
    private readonly IEan13SymbologyService _ean13SymbologyService;

    public SymbologyService(IEan13SymbologyService ean13SymbologyService)
    {
        _ean13SymbologyService = ean13SymbologyService;
    }
    
    public (bool isValid, string output) ValidateAndFormat(string input, SymbologyPropertiesDto properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case SymbologyDto.Ean13:
                return _ean13SymbologyService.ValidateAndFormat(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }
    
    public (bool isValid, string output) ValidateAndFormatFull(string input, SymbologyPropertiesDto properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case SymbologyDto.Ean13:
                return _ean13SymbologyService.ValidateAndFormatFull(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }
}