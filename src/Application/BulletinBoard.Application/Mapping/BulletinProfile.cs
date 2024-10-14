using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Domain.Entities;
using Mapster;

namespace BulletinBoard.Application.Mapping;

public class BulletinProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ICreateBulletinCommand, Bulletin>()
            .Map(dest => dest.Id, _ => Guid.NewGuid())
            .Map(dest => dest.CreatedUtc, _ => DateTime.UtcNow);

        config.NewConfig<IUpdateBulletinCommand, Bulletin>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedUtc);
    }
}