using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.AssetIdTypes.Base;

public class UpsertAssetIdTypeDto : INamedDto, IDescribedDto
{
    public string Name { get; set; }
        
    public string Description { get; set; }
    
    public bool WithManualEntry { get; set; }
    
    public SymbologyPropertiesDto Properties { get; set; }
}

public class UpsertAssetIdTypeDtoFieldValidator : CompositeValidator<UpsertAssetIdTypeDto>
{
    public UpsertAssetIdTypeDtoFieldValidator()
    {
        RegisterBaseValidator(new NamedFieldValidator());
        RegisterBaseValidator(new DescribedFieldValidator());
        
        RuleFor(x => x.Properties).SetInheritanceValidator(_ => {
            _.Add(new Ean13SymbologyPropertiesDtoFieldValidator());
        });
    }
}