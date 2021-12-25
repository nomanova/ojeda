namespace NomaNova.Ojeda.Utils.Services.Interfaces;

public interface IEan13SymbologyService
{
    bool IsValid(string input);

    bool IsValidFull(string input);

    (string output, string fullOutput) GenerateNext(string input);

    (bool isValid, string output) ValidateAndFormat(string input);
    
    (bool isValid, string fullOutput) ValidateAndFormatFull(string input);
}