using BulletinBoard.Application.Abstraction.Models.Queries;

namespace BulletinBoard.WebAPI.Models.Requests;

public class GetBulletinByIdRequest : IGetBulletinByIdQuery
{
    public Guid Id { get; init; }
}