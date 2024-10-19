using AutoMapper;
using Users.Domain.Entities;
using Users.Models.Responses;

namespace Users.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetUserByIdResponse>();

        CreateMap<User[], SearchUsersResponse>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src));
    }
}