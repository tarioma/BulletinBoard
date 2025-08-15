using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.SearchUsers;

public class SearchUsersQueryHandler(
    IUserRepository users)
    : IRequestHandler<SearchUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(SearchUsersQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        return await users.SearchAsync(request.SearchFilters, cancellationToken);
    }
}