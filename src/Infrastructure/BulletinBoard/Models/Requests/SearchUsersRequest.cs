using BulletinBoard.Application.Abstraction.Models.Queries;

namespace BulletinBoard.WebAPI.Models.Requests;

public class SearchUsersRequest : ISearchUsersQuery
{
    public int Page { get; init; } = 0;
    public int PageSize { get; init; } = 10;
    public string? Text { get; init; }
    public bool? IsAdmin { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
    public DateTimeOffset? CreatedFrom { get; init; }
    public DateTimeOffset? CreatedTo { get; init; }
}