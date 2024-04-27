namespace BulletinBoard.Application.Models.Users;

public record UsersSearchFilters(
    int Page,
    int Count,
    string? SearchName,
    bool? SearchIsAdmin,
    UsersSortBy SortBy,
    bool Desc,
    DateTime? CreatedFromUtc,
    DateTime? CreatedToUtc);