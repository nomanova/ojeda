using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetClasses;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.AssetClasses;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.AssetClasses
{
    public class AssetClassProfile : Profile
    {
        public AssetClassProfile()
        {
            // Domain -> Dto
            CreateMap<AssetClass, AssetClassSummaryDto>();
            CreateMap<AssetClass, AssetClassDto>()
                .ForMember(dest => dest.FieldSets, opt => opt.MapFrom(src => src.AssetClassFieldSets));
            CreateMap<PaginatedList<AssetClass>, PaginatedListDto<AssetClassDto>>();
            
            CreateMap<AssetClassFieldSet, AssetClassFieldSetDto>();
            
            // Dto -> Domain
            CreateMap<AssetClassDto, AssetClass>();
            CreateMap<AssetClassSummaryDto, AssetClass>();
            
            CreateMap<AssetClassFieldSetDto, AssetClassFieldSet>()
                .ForMember(dest => dest.AssetClass, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSet, opt => opt.Ignore())
                .ForMember(dest => dest.AssetClassId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSetId, opt => opt.MapFrom(src => src.FieldSet.Id));
        }
    }
}