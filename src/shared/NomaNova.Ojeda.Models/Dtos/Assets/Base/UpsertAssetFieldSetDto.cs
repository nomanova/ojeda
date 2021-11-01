using System.Collections.Generic;
using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetFieldSetDto<T> where T : UpsertAssetFieldDto
    {
        public string Id { get; set; }
        
        public List<T> Fields { get; set; }
    }
    
    public class UpsertAssetFieldSetDtoFieldValidator<T> : AbstractValidator<UpsertAssetFieldSetDto<T>>
        where T : UpsertAssetFieldDto
    {
        public UpsertAssetFieldSetDtoFieldValidator(IFieldPropertiesResolver fieldPropertiesResolver)
        {
            RuleForEach(_ => _.Fields).SetValidator(
                new UpsertAssetFieldDtoFieldValidator(fieldPropertiesResolver));
        }
    }
}