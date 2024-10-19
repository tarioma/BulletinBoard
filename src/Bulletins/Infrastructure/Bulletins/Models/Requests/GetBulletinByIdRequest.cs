using Bulletins.Application.Abstraction.Models.Queries;

namespace Bulletins.Models.Requests;

public class GetBulletinByIdRequest : IGetBulletinByIdQuery
{
    public Guid Id { get; init; }
}