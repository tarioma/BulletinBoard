using AutoMapper;
using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Responses;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetUserByIdResponse>();

        CreateMap<User[], SearchUsersResponse>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src));
    }
}