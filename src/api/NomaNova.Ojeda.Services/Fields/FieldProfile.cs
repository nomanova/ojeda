using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models.Dtos.Fields;
using NomaNova.Ojeda.Models.Dtos.Fields.Base;
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

            // Dto -> Domain
            CreateMap<CreateFieldDto, Field>();
            CreateMap<UpdateFieldDto, Field>();
        }
    }
}