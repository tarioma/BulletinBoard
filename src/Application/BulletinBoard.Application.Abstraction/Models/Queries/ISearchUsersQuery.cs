using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface ISearchUsersQuery : IRequest<User[]>
{
    int Page { get; }
    int PageSize { get; }
    string? Text { get; }
    bool? IsAdmin { get; }
    string? SortBy { get; }
    bool Desc { get; }
    DateTimeOffset? CreatedFrom { get; }
    DateTimeOffset? CreatedTo { get; }
}