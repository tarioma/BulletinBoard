using BulletinBoard.Application.Models.Users;

namespace BulletinBoard.Contracts.Users.Requests;

public record SearchUsersRequest(
    int Page = 0,
    int Count = 10,
    string? SearchName = null,
    bool? SearchIsAdmin = null,
    UsersSortBy SortBy = UsersSortBy.Default,
    bool Desc = false,
    DateTimeOffset? CreatedFrom = null,
    DateTimeOffset? CreatedTo = null);