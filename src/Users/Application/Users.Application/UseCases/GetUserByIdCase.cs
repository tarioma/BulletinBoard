using MediatR;
using Users.Application.Abstraction.Models.Queries;
using Users.Application.Abstraction.Repositories;
using Users.Domain.Entities;

namespace Users.Application.UseCases;

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