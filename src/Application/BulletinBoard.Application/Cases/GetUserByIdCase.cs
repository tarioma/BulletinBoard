using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class GetUserByIdCase : IRequestHandler<IGetUserByIdQuery, User>
{
    private readonly IUserRepository _users;

    public GetUserByIdCase(IUserRepository users)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
    }

    public async Task<User> Handle(IGetUserByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _users.GetByIdAsync(request.Id, cancellationToken);
    }
}