using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Responses;
using Mapster;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class BulletinMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Bulletin, GetBulletinByIdResponse>();

        config.NewConfig<Bulletin[], SearchBulletinsResponse>()
            .Map(dest => dest.Bulletins, src => src.Select(u => u.Adapt<GetBulletinByIdResponse>()).ToArray());
    }
}