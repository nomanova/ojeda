using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.Assets;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Assets
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            // Domain -> Dto
            CreateMap<Asset, AssetSummaryDto>();
            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.FieldSets, opt => opt.Ignore());
            CreateMap<FieldSet, AssetFieldSetDto>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Fields, opt => opt.Ignore());
            CreateMap<Field, AssetFieldDto>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Data, opt => opt.Ignore());

            CreateMap<PaginatedList<Asset>, PaginatedListDto<AssetSummaryDto>>();
            
            // Dto -> Domain
            CreateMap<CreateAssetDto, Asset>()
                .ForMember(dest => dest.AssetType, opt => opt.Ignore())
                .ForMember(dest => dest.FieldValues, opt => opt.Ignore());
            
            CreateMap<UpdateAssetDto, Asset>()
                .ForMember(dest => dest.AssetType, opt => opt.Ignore())
                .ForMember(dest => dest.FieldValues, opt => opt.Ignore());
        }
    }
}