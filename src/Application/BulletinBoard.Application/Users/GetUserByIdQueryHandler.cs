using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users;

public class GetUserByIdQueryHandler : IRequestHandler<IGetUserByIdQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserByIdQueryHandler(IUserRepository users)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
    }

    public async Task<User> Handle(IGetUserByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _users.GetByIdAsync(request.Id, cancellationToken);
    }
}