using BulletinBoard.Application.Models.Bulletins;

namespace BulletinBoard.Contracts.Bulletins.Requests;

public record SearchBulletinsRequest(
    int Page = 0,
    int Count = 10,
    string? SearchNumber = null,
    string? SearchText = null,
    Guid? SearchUserId = null,
    BulletinsSortBy SortBy = BulletinsSortBy.Default,
    bool Desc = false,
    int? RatingFrom = null,
    int? RatingTo = null,
    DateTimeOffset? CreatedFrom = null,
    DateTimeOffset? CreatedTo = null,
    DateTimeOffset? ExpiryFrom = null,
    DateTimeOffset? ExpiryTo = null);