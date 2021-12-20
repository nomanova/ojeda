using System;
using Microsoft.AspNetCore.Components;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Web.Shared.Base;

namespace NomaNova.Ojeda.Web.Features.Admin.AssetIdTypes;

public abstract class AssetIdTypePage : FormPage<AssetIdTypeDto>
{
    protected void OnSymbologyChange(ChangeEventArgs args)
    {
        if (args.Value != null)
        {
            var symbology = Enum.Parse<SymbologyDto>((string)args.Value);

            switch (symbology)
            {
                case SymbologyDto.Ean13:
                    SetSymbologyProperties(new Ean13SymbologyPropertiesDto());
                    break;
                default:
                    throw new NotImplementedException(nameof(symbology));
            }
        }
    }

    protected abstract void SetSymbologyProperties(SymbologyPropertiesDto symbologyProperties);
}