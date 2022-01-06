using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Shared;

namespace NomaNova.Ojeda.Services.Features.Fields;

public class FieldProfile : Profile
{
    public FieldProfile()
    {
        // Domain -> Dto
        CreateMap<Field, FieldDto>();
        CreateMap<PaginatedList<Field>, PaginatedListDto<FieldDto>>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<FieldProperties, FieldPropertiesDto>()
            .IncludeAllDerived();
        CreateMap<TextFieldProperties, TextFieldPropertiesDto>();
        CreateMap<NumberFieldProperties, NumberFieldPropertiesDto>();

        // Dto -> Domain
        CreateMap<CreateFieldDto, Field>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FieldSetFields, opt => opt.Ignore())
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore());

        CreateMap<UpdateFieldDto, Field>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.FieldSetFields, opt => opt.Ignore())
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore())
            .ForMember(dest => dest.Properties, opt => opt.Ignore());

        CreateMap<FieldPropertiesDto, FieldProperties>()
            .IncludeAllDerived();
        CreateMap<TextFieldPropertiesDto, TextFieldProperties>();
        CreateMap<NumberFieldPropertiesDto, NumberFieldProperties>();
    }
}