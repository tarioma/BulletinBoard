using BulletinBoard.Application.SearchFilters;

namespace BulletinBoard.Application.Models.Users;

public record UsersSearchFilters(
    PageFilter Page,
    string? SearchName,
    bool? SearchIsAdmin,
    UsersSortBy SortBy,
    bool Desc,
    DateRangeFilters Created);