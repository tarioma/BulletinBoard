using BulletinBoard.Application.Abstraction.Models.SearchFilters;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface ISearchBulletinsQuery : IRequest<Bulletin[]>
{
    PageFilter Page { get; }
    int? Number { get; }
    string? Text { get; }
    Guid? UserId { get; }
    string? SortBy { get; }
    bool Desc { get; }
    DateRangeFilters Created { get; }
    DateRangeFilters Expiry { get; }
}