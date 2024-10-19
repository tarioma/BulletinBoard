using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface ISearchBulletinsQuery : IRequest<Bulletin[]>
{
    int Page { get; }
    int PageSize { get; }
    int? Number { get; }
    string? Text { get; }
    Guid? UserId { get; }
    string? SortBy { get; }
    bool Desc { get; }
    DateTimeOffset? CreatedFrom { get; }
    DateTimeOffset? CreatedTo { get; }
    DateTimeOffset? ExpiryFrom { get; }
    DateTimeOffset? ExpiryTo { get; }
}