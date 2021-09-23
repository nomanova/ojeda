using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.AssetTypes.Base
{
    public abstract class UpsertAssetTypeDto<T> : INamedDto where T : UpsertAssetTypeFieldSetDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<T> FieldSets { get; set; }
    }

    public abstract class UpsertAssetTypeFieldSetDto : IIdentityDto
    {
        public string Id { get; set; }
        
        public int Order { get; set; }
    }

    public class UpsertAssetTypeDtoFieldValidator<T> : 
        CompositeValidator<UpsertAssetTypeDto<T>> where T : UpsertAssetTypeFieldSetDto
    {
        public UpsertAssetTypeDtoFieldValidator()
        {
            RegisterBaseValidator(new NamedFieldValidator());
            
            RuleFor(_ => _.FieldSets).NotEmpty().WithMessage("At least one field set is required.");
            
            RuleFor(_ => _.FieldSets).Must(fieldSets =>
            {
                var fieldSetIds = fieldSets.Select(f => f.Id).ToList();
                return fieldSetIds.Count == fieldSetIds.Distinct().Count();
            }).WithMessage("A field set must not be added more than once.");
            
            RuleForEach(_ => _.FieldSets).SetValidator(new UpsertAssetTypeFieldSetDtoValidator());
        }
    }

    public class UpsertAssetTypeFieldSetDtoValidator : AbstractValidator<UpsertAssetTypeFieldSetDto>
    {
        public UpsertAssetTypeFieldSetDtoValidator()
        {
            RuleFor(_ => _.Order).GreaterThanOrEqualTo(0);
            RuleFor(_ => _.Id).NotEmpty().WithMessage("Field set id is missing.");
        }
    }
}