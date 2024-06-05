using BulletinBoard.Application.Models.Users;
using BulletinBoard.Contracts.Extensions;

namespace BulletinBoard.Contracts.Users.Requests;

public record SearchUsersRequest(
    int Count = SearchUsersRequest.DefaultCount,
    int Offset = 0,
    string? Name = null,
    bool? IsAdmin = null,
    UsersSortBy SortBy = UsersSortBy.Default,
    bool Desc = false,
    DateTimeOffset? CreatedFrom = null,
    DateTimeOffset? CreatedTo = null)
{
    private const int DefaultCount = 10;
    private const int MinCount = 1;
    private const int MaxCount = 100;

    public int Count { get; } = int.Clamp(Count, MinCount, MaxCount);
    public int Offset { get; } = Offset.ClampMin(0);
}