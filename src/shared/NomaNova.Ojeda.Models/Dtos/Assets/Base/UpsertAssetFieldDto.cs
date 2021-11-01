using FluentValidation;

namespace NomaNova.Ojeda.Models.Dtos.Assets.Base
{
    public abstract class UpsertAssetFieldDto
    {
        public string Id { get; set; }

        public FieldDataDto Data { get; set; }
    }

    public class UpsertAssetFieldDtoFieldValidator : AbstractValidator<UpsertAssetFieldDto>
    {
        public UpsertAssetFieldDtoFieldValidator(IFieldPropertiesResolver fieldPropertiesResolver)
        {
            RuleFor(x => x.Data).SetInheritanceValidator(_ =>
            {
                _.Add(dto =>
                {
                    var fieldProperties = fieldPropertiesResolver.Resolve(dto.Id);
                    return new StringFieldDataDtoFieldValidator(fieldProperties);
                });

                _.Add(dto => {
                    var fieldProperties = fieldPropertiesResolver.Resolve(dto.Id);
                    return new LongFieldDataDtoFieldValidator(fieldProperties);
                });
                
                _.Add(dto => 
                {
                    var fieldProperties = fieldPropertiesResolver.Resolve(dto.Id);
                    return new DoubleFieldDataDtoFieldValidator(fieldProperties);
                });
            });
        }
    }
}