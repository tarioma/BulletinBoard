using AutoMapper;
using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.MappingProfiles;

public class BulletinMappingProfile : Profile
{
    public BulletinMappingProfile()
    {
        CreateMap<ICreateBulletinCommand, Bulletin>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedUtc, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<IUpdateBulletinCommand, Bulletin>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore());
    }
}