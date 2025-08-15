using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.SearchBulletins;

public class SearchBulletinsQueryHandler(
    IBulletinRepository bulletins)
    : IRequestHandler<SearchBulletinsQuery, IEnumerable<Bulletin>>
{
    public async Task<IEnumerable<Bulletin>> Handle(
        SearchBulletinsQuery request,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        return await bulletins.SearchAsync(request.SearchFilters, cancellationToken);
    }
}