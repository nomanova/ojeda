using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

namespace NomaNova.Ojeda.Web.Features.App.Assets.Services.Interfaces;

public interface ISymbologyService
{
    (bool isValid, string output) ValidateAndFormat(string input, SymbologyPropertiesDto properties);

    (bool isValid, string output) ValidateAndFormatFull(string input, SymbologyPropertiesDto properties);
}