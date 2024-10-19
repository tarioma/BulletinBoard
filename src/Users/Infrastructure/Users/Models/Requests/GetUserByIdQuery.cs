using Users.Application.Abstraction.Models.Queries;

namespace Users.Models.Requests;

public class GetUserByIdQuery : IGetUserByIdQuery
{
    public Guid Id { get; init; }
}