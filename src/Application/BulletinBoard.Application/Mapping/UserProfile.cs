using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Domain.Entities;
using Mapster;

namespace BulletinBoard.Application.Mapping;

public class UserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ICreateUserCommand, User>()
            .Map(dest => dest.Id, _ => Guid.NewGuid())
            .Map(dest => dest.CreatedUtc, _ => DateTime.UtcNow);

        config.NewConfig<IUpdateUserCommand, User>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedUtc);
    }
}