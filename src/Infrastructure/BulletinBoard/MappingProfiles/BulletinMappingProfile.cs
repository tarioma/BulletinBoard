using AutoMapper;
using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Responses;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class BulletinMappingProfile : Profile
{
    public BulletinMappingProfile()
    {
        CreateMap<Bulletin, GetBulletinByIdResponse>();

        CreateMap<Bulletin[], SearchBulletinsResponse>()
            .ForMember(dest => dest.Bulletins, opt => opt.MapFrom(src => src));
    }
}