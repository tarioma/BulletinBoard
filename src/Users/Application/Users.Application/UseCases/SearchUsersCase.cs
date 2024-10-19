using MediatR;
using Users.Application.Abstraction.Models.Queries;
using Users.Application.Abstraction.Repositories;
using Users.Domain.Entities;

namespace Users.Application.UseCases;

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