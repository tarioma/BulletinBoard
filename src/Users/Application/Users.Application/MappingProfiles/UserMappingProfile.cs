using AutoMapper;
using Users.Application.Abstraction.Models.Commands;
using Users.Domain.Entities;

namespace Users.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ICreateUserCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedUtc, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<IUpdateUserCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore());
    }
}