using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class SearchUsersCase : IRequestHandler<ISearchUsersQuery, User[]>
{
    private readonly IUserRepository _users;

    public SearchUsersCase(IUserRepository users)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
    }

    public async Task<User[]> Handle(ISearchUsersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _users.SearchAsync(
            request.Page,
            request.PageSize,
            request.Text,
            request.IsAdmin,
            request.SortBy,
            request.Desc,
            request.CreatedFrom?.UtcDateTime,
            request.CreatedTo?.UtcDateTime,
            cancellationToken);
    }
}