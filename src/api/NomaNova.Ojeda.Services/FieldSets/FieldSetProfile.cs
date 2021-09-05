using AutoMapper;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.FieldSets;

namespace NomaNova.Ojeda.Services.FieldSets
{
    public class FieldSetProfile : Profile
    {
        public FieldSetProfile()
        {
            CreateMap<FieldSet, FieldSetDto>()
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.FieldSetFields));
            CreateMap<PaginatedList<FieldSet>, PaginatedListDto<FieldSetDto>>();
            
            CreateMap<FieldSetField, FieldSetFieldDto>();
        }
    }
}