namespace BulletinBoard.Application.SearchFilters.Bulletins;

public record BulletinsSearchFilters(
    PageFilter Page,
    string? SearchNumber,
    string? SearchText,
    Guid? SearchUserId,
    BulletinsSortBy SortBy,
    bool Desc,
    BulletinRatingFilters Rating,
    DateRangeFilters Created,
    DateRangeFilters Expiry);