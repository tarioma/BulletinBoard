using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.SearchUsers;

public class SearchUsersQuery : IRequest<IEnumerable<User>>
{
    public SearchUsersQuery(UsersSearchFilters searchFilters)
    {
        Guard.Against.Null(searchFilters);

        SearchFilters = searchFilters;
    }

    public UsersSearchFilters SearchFilters { get; }
}