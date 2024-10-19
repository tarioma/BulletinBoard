using AutoMapper;
using Bulletins.Domain.Entities;
using Bulletins.Models.Responses;

namespace Bulletins.MappingProfiles;

public class BulletinMappingProfile : Profile
{
    public BulletinMappingProfile()
    {
        CreateMap<Bulletin, GetBulletinByIdResponse>();

        CreateMap<Bulletin[], SearchBulletinsResponse>()
            .ForMember(dest => dest.Bulletins, opt => opt.MapFrom(src => src));
    }
}