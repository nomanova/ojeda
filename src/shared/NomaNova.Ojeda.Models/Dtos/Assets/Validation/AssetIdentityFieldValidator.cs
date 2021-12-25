using System;
using System.Text.RegularExpressions;
using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Validation;

public class AssetIdentityFieldValidator : AbstractValidator<IAssetIdentityDto>
{
    public AssetIdentityFieldValidator(SymbologyPropertiesDto symbologyProperties)
    {
        var symbology = symbologyProperties.Symbology;

        RuleFor(_ => _.AssetId).Must(assetId =>
        {
            switch (symbology)
            {
                case SymbologyDto.Ean13:
                    var regex = new Regex("^[0-9]{1,12}$");
                    return !string.IsNullOrEmpty(assetId) && regex.IsMatch(assetId);
                default:
                    throw new NotImplementedException(nameof(symbology));
            }
        }).WithMessage(_ =>
        {
            switch (symbology)
            {
                case SymbologyDto.Ean13:
                    return "Not a valid EAN13 number.";
                default:
                    throw new NotImplementedException(nameof(symbology));
            }
        });
    }
}