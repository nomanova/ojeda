using System.Collections.Generic;
using NomaNova.Ojeda.Models.Shared.Interfaces;
using NomaNova.Ojeda.Models.Shared.Validation;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetDto<T, TS> : INamedDto
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public string Name { get; set; }

        public string AssetTypeId { get; set; }

        public List<T> FieldSets { get; set; }
    }

    public class UpsertAssetDtoFieldValidator<T, TS> : CompositeValidator<UpsertAssetDto<T, TS>>
        where T : UpsertAssetFieldSetDto<TS> where TS : UpsertAssetFieldDto
    {
        public UpsertAssetDtoFieldValidator(IFieldPropertiesResolver fieldPropertiesResolver)
        {
            RegisterBaseValidator(new NamedFieldValidator());

            RuleForEach(_ => _.FieldSets).SetValidator(
                new UpsertAssetFieldSetDtoFieldValidator<TS>(fieldPropertiesResolver));
        }
    }
}