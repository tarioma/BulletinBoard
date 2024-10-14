using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Models.SearchFilters;

namespace BulletinBoard.WebAPI.Models.Requests;

public class SearchBulletinsRequest : ISearchBulletinsQuery
{
    public PageFilter Page { get; init; } = null!;
    public int? Number { get; init; }
    public string? Text { get; init; }
    public Guid? UserId { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
    public DateRangeFilters Created { get; init; } = null!;
    public DateRangeFilters Expiry { get; init; } = null!;
}