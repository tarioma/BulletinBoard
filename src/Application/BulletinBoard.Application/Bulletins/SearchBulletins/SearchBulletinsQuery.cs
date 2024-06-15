using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.SearchBulletins;

public class SearchBulletinsQuery : IRequest<IEnumerable<Bulletin>>
{
    public SearchBulletinsQuery(BulletinsSearchFilters searchFilters)
    {
        Guard.Against.Null(searchFilters);

        SearchFilters = searchFilters;
    }

    public BulletinsSearchFilters SearchFilters { get; }
}