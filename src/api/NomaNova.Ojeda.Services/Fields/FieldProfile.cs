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

            CreateMap<FieldData, FieldDataDto>().IncludeAllDerived();
            CreateMap<TextFieldData, TextFieldDataDto>();
            CreateMap<NumberFieldData, NumberFieldDataDto>();
            
            // Dto -> Domain
            CreateMap<CreateFieldDto, Field>();
            CreateMap<UpdateFieldDto, Field>()
                .ForMember(dest => dest.Data, opt => opt.Ignore());
            
            CreateMap<FieldDataDto, FieldData>().IncludeAllDerived();
            CreateMap<TextFieldDataDto, TextFieldData>();
            CreateMap<NumberFieldDataDto, NumberFieldData>();
        }
    }
}