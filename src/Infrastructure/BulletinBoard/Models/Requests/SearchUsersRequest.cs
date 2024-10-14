using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Models.SearchFilters;

namespace BulletinBoard.WebAPI.Models.Requests;

public class SearchUsersRequest : ISearchUsersQuery
{
    public PageFilter Page { get; init; } = null!;
    public string? Text { get; init; }
    public bool? IsAdmin { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
    public DateRangeFilters Created { get; init; } = null!;
}