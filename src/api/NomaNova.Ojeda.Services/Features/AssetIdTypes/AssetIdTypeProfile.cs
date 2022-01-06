using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Features.AssetIdTypes;

public class AssetIdTypeProfile : Profile
{
    public AssetIdTypeProfile()
    {
        // Domain -> Dto
        CreateMap<AssetIdType, NamedEntityDto>();
        CreateMap<AssetIdType, AssetIdTypeSummaryDto>();
        CreateMap<AssetIdType, AssetIdTypeDto>();
        CreateMap<PaginatedList<AssetIdType>, PaginatedListDto<AssetIdTypeDto>>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<SymbologyProperties, SymbologyPropertiesDto>().IncludeAllDerived();
        CreateMap<Ean13SymbologyProperties, Ean13SymbologyPropertiesDto>();

        // Dto -> Domain
        CreateMap<CreateAssetIdTypeDto, AssetIdType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AssetTypes, opt => opt.Ignore());
            
        CreateMap<UpdateAssetIdTypeDto, AssetIdType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AssetTypes, opt => opt.Ignore())
            .ForMember(dest => dest.Properties, opt => opt.Ignore());

        CreateMap<SymbologyPropertiesDto, SymbologyProperties>().IncludeAllDerived();
        CreateMap<Ean13SymbologyPropertiesDto, Ean13SymbologyProperties>();
    }
}