using NomaNova.Ojeda.Core.Domain.AssetIdTypes;

namespace NomaNova.Ojeda.Services.AssetIds.Interfaces;

public interface ISymbologyService
{
    (bool isValid, string output) ValidateAndFormat(string input, SymbologyProperties properties);
    
    (bool isValid, string fullOutput) ValidateAndFormatFull(string input, SymbologyProperties properties);
    
    bool IsValidFull(string input, SymbologyProperties properties);

    (string output, string fullOutput) GenerateNext(string input, SymbologyProperties properties);
}