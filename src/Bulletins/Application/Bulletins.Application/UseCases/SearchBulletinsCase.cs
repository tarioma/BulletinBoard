using Bulletins.Application.Abstraction.Models.Queries;
using Bulletins.Application.Abstraction.Repositories;
using Bulletins.Domain.Entities;
using MediatR;

namespace Bulletins.Application.UseCases;

public class SearchBulletinsCase : IRequestHandler<ISearchBulletinsQuery, Bulletin[]>
{
    private readonly IBulletinRepository _bulletins;

    public SearchBulletinsCase(IBulletinRepository bulletins)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
    }

    public async Task<Bulletin[]> Handle(ISearchBulletinsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _bulletins.SearchAsync(
            request.Page,
            request.PageSize,
            request.Number,
            request.Text,
            request.UserId,
            request.SortBy,
            request.Desc,
            request.CreatedFrom?.UtcDateTime,
            request.CreatedTo?.UtcDateTime,
            request.ExpiryFrom?.UtcDateTime,
            request.ExpiryTo?.UtcDateTime,
            cancellationToken);
    }
}