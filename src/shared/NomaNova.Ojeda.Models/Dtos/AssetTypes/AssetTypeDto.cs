using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Dtos.FieldSets;
using NomaNova.Ojeda.Models.Shared.Interfaces;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes
{
    public class AssetTypeSummaryDto : IIdentityDto, INamedDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }   
    }

    public class AssetTypeDto : AssetTypeSummaryDto
    {
        public List<AssetTypeFieldSetDto> FieldSets { get; set; } = new();
    }

    public class AssetTypeDtoValidator : AbstractValidator<AssetTypeDto>
    {
        public AssetTypeDtoValidator()
        {
            RuleFor(_ =>  _.Name).NotEmpty();
            RuleFor(_ =>  _.Name).MaximumLength(40);
            RuleFor(_ =>  _.Description).MaximumLength(250);
            RuleFor(_ => _.FieldSets).NotEmpty().WithMessage("At least one field set is required.");
            
            RuleFor(_ => _.FieldSets).Must(fieldSets =>
            {
                var fieldSetIds = fieldSets.Select(f => f.FieldSet.Id).ToList();
                return fieldSetIds.Count == fieldSetIds.Distinct().Count();
            }).WithMessage("A field set must not be added more than once.");
            
            RuleForEach(_ => _.FieldSets).SetValidator(new AssetTypeFieldSetDtoValidator());
        }
    }

    public class AssetTypeFieldSetDto
    {
        public int Order { get; set; }

        public FieldSetSummaryDto FieldSet { get; set; }
    }

    public class AssetTypeFieldSetDtoValidator : AbstractValidator<AssetTypeFieldSetDto>
    {
        public AssetTypeFieldSetDtoValidator()
        {
            RuleFor(_ => _.Order).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.FieldSet.Id).NotEmpty().WithMessage("Field set id is missing.");
        }
    }
}