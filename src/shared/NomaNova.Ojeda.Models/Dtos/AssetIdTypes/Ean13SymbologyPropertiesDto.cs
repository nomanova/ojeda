using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes;

public class Ean13SymbologyPropertiesDto : SymbologyPropertiesDto
{
    public Ean13SymbologyPropertiesDto()
    {
        Symbology = SymbologyDto.Ean13;
    }
}

public class Ean13SymbologyPropertiesDtoFieldValidator : AbstractValidator<Ean13SymbologyPropertiesDto>
{
    public Ean13SymbologyPropertiesDtoFieldValidator()
    {
    }
}