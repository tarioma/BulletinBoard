using BulletinBoard.Application.Abstraction.Models.Queries;

namespace BulletinBoard.WebAPI.Models.Requests;

public class GetUserByIdQuery : IGetUserByIdQuery
{
    public Guid Id { get; init; }
}