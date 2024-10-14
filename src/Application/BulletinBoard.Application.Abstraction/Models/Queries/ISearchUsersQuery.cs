using BulletinBoard.Application.Abstraction.Models.SearchFilters;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface ISearchUsersQuery : IRequest<User[]>
{
    PageFilter Page { get; }
    string? Text { get; }
    bool? IsAdmin { get; }
    string? SortBy { get; }
    bool Desc { get; }
    DateRangeFilters Created { get; }
}