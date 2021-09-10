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
            // Domain -> Dto
            CreateMap<FieldSet, FieldSetSummaryDto>();
            CreateMap<FieldSet, FieldSetDto>()
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.FieldSetFields));
            CreateMap<PaginatedList<FieldSet>, PaginatedListDto<FieldSetDto>>();
            
            CreateMap<FieldSetField, FieldSetFieldDto>();
            
            // Dto -> Domain
            CreateMap<FieldSetDto, FieldSet>();
            
            CreateMap<FieldSetFieldDto, FieldSetField>()
                .ForMember(dest => dest.Field, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSet, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSetId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => src.Field.Id));
        }
    }
}