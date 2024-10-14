using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Responses;
using Mapster;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserByIdResponse>();

        config.NewConfig<User[], SearchUsersResponse>()
            .Map(dest => dest.Users, src => src.Select(u => u.Adapt<GetUserByIdResponse>()).ToArray());
    }
}