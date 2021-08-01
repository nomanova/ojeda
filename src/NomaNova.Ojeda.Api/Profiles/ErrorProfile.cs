using AutoMapper;
using NomaNova.Ojeda.Api.Models;
using NomaNova.Ojeda.Models;

namespace NomaNova.Ojeda.Api.Profiles
{
    public class ErrorProfile : Profile
    {
        public ErrorProfile()
        {
            CreateMap<Error, ErrorDto>();
        }
    }
}