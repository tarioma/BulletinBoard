namespace BulletinBoard.Application.SearchFilters.Users;

public record UsersSearchFilters(
    PageFilter Page,
    string? SearchName,
    bool? SearchIsAdmin,
    UsersSortBy SortBy,
    bool Desc,
    DateRangeFilters Created);