using System.Collections.Generic;
using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetDto <T, TS> 
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public string AssetTypeId { get; set; }

        public List<T> FieldSets { get; set; }
    }

    public class UpsertAssetDtoFieldValidator<T, TS> : AbstractValidator<UpsertAssetDto<T, TS>> 
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public UpsertAssetDtoFieldValidator(IFieldPropertiesResolver fieldPropertiesResolver)
        {
            RuleForEach(_ => _.FieldSets).SetValidator(
                new UpsertAssetFieldSetDtoFieldValidator<TS>(fieldPropertiesResolver));
        }
    }
}