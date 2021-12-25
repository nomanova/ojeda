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
            CreateMap<AssetType, NamedEntityDto>();
            CreateMap<AssetType, AssetTypeSummaryDto>();
            CreateMap<AssetType, AssetTypeDto>()
                .ForMember(dest => dest.FieldSets, opt => opt.MapFrom(src => src.AssetTypeFieldSets));
            CreateMap<PaginatedList<AssetType>, PaginatedListDto<AssetTypeDto>>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            
            CreateMap<AssetTypeFieldSet, AssetTypeFieldSetDto>();
            
            // Dto -> Domain
            CreateMap<CreateAssetTypeDto, AssetType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AssetIdType, opt => opt.Ignore())
                .ForMember(dest => dest.AssetTypeFieldSets, opt => opt.Ignore())
                .ForMember(dest => dest.Assets, opt => opt.Ignore());
                
            CreateMap<UpdateAssetTypeDto, AssetType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AssetIdType, opt => opt.Ignore())
                .ForMember(dest => dest.AssetTypeFieldSets, opt => opt.Ignore())
                .ForMember(dest => dest.Assets, opt => opt.Ignore());
            
            CreateMap<CreateAssetTypeFieldSetDto, AssetTypeFieldSet>()
                .ForMember(dest => dest.AssetType, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSet, opt => opt.Ignore())
                .ForMember(dest => dest.AssetTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSetId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<UpdateAssetTypeFieldSetDto, AssetTypeFieldSet>()
                .ForMember(dest => dest.AssetType, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSet, opt => opt.Ignore())
                .ForMember(dest => dest.AssetTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldSetId, opt => opt.MapFrom(src => src.Id));
        }
    }
}