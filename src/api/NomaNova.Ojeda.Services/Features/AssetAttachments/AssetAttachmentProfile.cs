using AutoMapper;
using NomaNova.Ojeda.Core.Domain.AssetAttachments;
using NomaNova.Ojeda.Models.Dtos.AssetAttachments;

namespace NomaNova.Ojeda.Services.Features.AssetAttachments;

public class AssetAttachmentProfile : Profile
{
    public AssetAttachmentProfile()
    {
        // Domain -> Dto
        CreateMap<AssetAttachment, AssetAttachmentDto>();
    }
}