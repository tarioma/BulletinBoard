using BulletinBoard.Application.SearchFilters;

namespace BulletinBoard.Application.Models.Users;

public record UsersSearchFilters(
    PageFilter Page,
    string? SearchName,
    bool? SearchIsAdmin,
    string? SortBy,
    bool Desc,
    DateRangeFilters Created);