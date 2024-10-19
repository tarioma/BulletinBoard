using BulletinBoard.Application.Abstraction.Models.Queries;

namespace BulletinBoard.WebAPI.Models.Requests;

public class SearchBulletinsRequest : ISearchBulletinsQuery
{
    public int Page { get; init; } = 0;
    public int PageSize { get; init; } = 10;
    public int? Number { get; init; }
    public string? Text { get; init; }
    public Guid? UserId { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
    public DateTimeOffset? CreatedFrom { get; init; }
    public DateTimeOffset? CreatedTo { get; init; }
    public DateTimeOffset? ExpiryFrom { get; init; }
    public DateTimeOffset? ExpiryTo { get; init; }
}