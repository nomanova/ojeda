using AutoMapper;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Data;
using NomaNova.Ojeda.Models;
using NomaNova.Ojeda.Models.Fields;

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
            CreateMap<FieldDto, Field>();
        }
    }
}