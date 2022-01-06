using System;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Services.Features.AssetIds.Interfaces;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Services.Features.AssetIds;

public class SymbologyService : ISymbologyService
{
    private readonly IEan13SymbologyService _ean13SymbologyService;
    
    public SymbologyService(IEan13SymbologyService ean13SymbologyService)
    {
        _ean13SymbologyService = ean13SymbologyService;
    }

    public (bool isValid, string output) ValidateAndFormat(string input, SymbologyProperties properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case Symbology.Ean13:
                return _ean13SymbologyService.ValidateAndFormat(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }
    
    public (bool isValid, string fullOutput) ValidateAndFormatFull(string input, SymbologyProperties properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case Symbology.Ean13:
                return _ean13SymbologyService.ValidateAndFormatFull(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }

    public bool IsValidFull(string input, SymbologyProperties properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case Symbology.Ean13:
                return _ean13SymbologyService.IsValidFull(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }

    public (string output, string fullOutput) GenerateNext(string input, SymbologyProperties properties)
    {
        var symbology = properties.Symbology;

        switch (symbology)
        {
            case Symbology.Ean13:
                return _ean13SymbologyService.GenerateNext(input);
            default:
                throw new NotImplementedException(symbology.ToString());
        }
    }
}