using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetIdTypes;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.AssetIdTypes;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.AssetIdTypes;

public class AssetIdTypeProfile : Profile
{
    public AssetIdTypeProfile()
    {
        // Domain -> Dto
        CreateMap<AssetIdType, AssetIdTypeDto>();
        CreateMap<PaginatedList<AssetIdType>, PaginatedListDto<AssetIdTypeDto>>();

        CreateMap<SymbologyProperties, SymbologyPropertiesDto>().IncludeAllDerived();
        CreateMap<Ean13SymbologyProperties, Ean13SymbologyPropertiesDto>();

        // Dto -> Domain
        CreateMap<CreateAssetIdTypeDto, AssetIdType>();
        CreateMap<UpdateAssetIdTypeDto, AssetIdType>()
            .ForMember(dest => dest.Properties, opt => opt.Ignore());

        CreateMap<SymbologyPropertiesDto, SymbologyProperties>().IncludeAllDerived();
        CreateMap<Ean13SymbologyPropertiesDto, Ean13SymbologyProperties>();
    }
}