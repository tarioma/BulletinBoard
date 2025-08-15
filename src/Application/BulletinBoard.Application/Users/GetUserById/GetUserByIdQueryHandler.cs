using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository users)
    : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        return await users.GetByIdAsync(request.Id, cancellationToken);
    }
}