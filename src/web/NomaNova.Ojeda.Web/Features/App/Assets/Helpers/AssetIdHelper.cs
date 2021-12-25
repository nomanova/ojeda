using System;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Web.Features.App.Assets.Services.Interfaces;

namespace NomaNova.Ojeda.Web.Features.App.Assets.Helpers;

public static class AssetIdHelper
{
    public static (string helpText, string placeholder) GetUserInfo(AssetIdTypeDto assetIdType)
    {
        var withManualEntry = assetIdType.WithManualEntry;
        var symbology = assetIdType.Properties.Symbology;
                                        
        string helpText;
        string placeholder = null;

        if (withManualEntry)
        {
            switch (symbology)
            {
                case SymbologyDto.Ean13:
                    helpText = "Enter at most 12 digits to form a valid EAN13 number.<br>The check digit will be calculated automatically when submitted.";
                    placeholder = "123456789012";
                    break;
                default:
                    throw new NotImplementedException(nameof(symbology));
            }
        }
        else
        {
            helpText = "Manual entry of the asset id is disabled.";
        }

        return (helpText, placeholder);
    }

    public static string ToMinimalForm(
        string assetId, SymbologyPropertiesDto symbologyProperties, ISymbologyService symbologyService)
    {
        var (isValid, output) = symbologyService.ValidateAndFormat(assetId, symbologyProperties);
        return isValid ? output : assetId;
    }

    public static string ToFullForm(
        string assetId, SymbologyPropertiesDto symbologyProperties, ISymbologyService symbologyService)
    {
        var (isValid, output) = symbologyService.ValidateAndFormatFull(assetId, symbologyProperties);
        return isValid ? output : assetId;
    }
}