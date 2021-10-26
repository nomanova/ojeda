using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Fields
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            // Domain -> Dto
            CreateMap<Field, FieldDto>();
            CreateMap<PaginatedList<Field>, PaginatedListDto<FieldDto>>();

            CreateMap<FieldProperties, FieldPropertiesDto>().IncludeAllDerived();
            CreateMap<TextFieldProperties, TextFieldPropertiesDto>();
            CreateMap<NumberFieldProperties, NumberFieldPropertiesDto>();
            
            // Dto -> Domain
            CreateMap<CreateFieldDto, Field>();
            CreateMap<UpdateFieldDto, Field>()
                .ForMember(dest => dest.Properties, opt => opt.Ignore());
            
            CreateMap<FieldPropertiesDto, FieldProperties>().IncludeAllDerived();
            CreateMap<TextFieldPropertiesDto, TextFieldProperties>();
            CreateMap<NumberFieldPropertiesDto, NumberFieldProperties>();
        }
    }
}