namespace BulletinBoard.Application.Models.Bulletins;

public record BulletinsSearchFilters(
    int Page,
    int Count,
    string? SearchNumber,
    string? SearchText,
    Guid? SearchUserId,
    BulletinsSortBy SortBy,
    bool Desc,
    int? RatingFrom,
    int? RatingTo,
    DateTime? CreatedFromUtc,
    DateTime? CreatedToUtc,
    DateTime? ExpiryFromUtc,
    DateTime? ExpiryToUtc);