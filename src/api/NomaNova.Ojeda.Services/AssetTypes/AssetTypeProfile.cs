using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.AssetTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.AssetTypes
{
    public class AssetTypeProfile : Profile
    {
        public AssetTypeProfile()
        {
            // Domain -> Dto
            CreateMap<AssetType, AssetTypeSummaryDto>();
            CreateMap<AssetType, AssetTypeDto>()
                .ForMember(dest => dest.FieldSets, opt => opt.MapFrom(src => src.AssetTypeFieldSets));
            CreateMap<PaginatedList<AssetType>, PaginatedListDto<AssetTypeDto>>();
            
            CreateMap<AssetTypeFieldSet, AssetTypeFieldSetDto>();
            
            // Dto -> Domain
            CreateMap<AssetTypeDto, AssetType>();
            CreateMap<AssetTypeSummaryDto, AssetType>();
            
            CreateMap<AssetTypeFieldSetDto, AssetTypeFieldSet>()
                .ForMember(dest => dest.AssetType, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSet, opt => opt.Ignore())
                .ForMember(dest => dest.AssetTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSetId, opt => opt.MapFrom(src => src.FieldSet.Id));
        }
    }
}