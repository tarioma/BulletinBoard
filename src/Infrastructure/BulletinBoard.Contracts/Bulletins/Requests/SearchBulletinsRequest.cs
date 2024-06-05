using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Contracts.Extensions;

namespace BulletinBoard.Contracts.Bulletins.Requests;

public record SearchBulletinsRequest(
    int Count = SearchBulletinsRequest.DefaultCount,
    int Offset = 0,
    string? Text = null,
    Guid? UserId = null,
    BulletinsSortBy SortBy = BulletinsSortBy.Default,
    bool Desc = false,
    int? RatingFrom = null,
    int? RatingTo = null,
    DateTimeOffset? CreatedFrom = null,
    DateTimeOffset? CreatedTo = null,
    DateTimeOffset? ExpiryFrom = null,
    DateTimeOffset? ExpiryTo = null)
{
    private const int DefaultCount = 10;
    private const int MinCount = 1;
    private const int MaxCount = 100;

    public int Count { get; } = int.Clamp(Count, MinCount, MaxCount);
    public int Offset { get; } = Offset.ClampMin(0);
}