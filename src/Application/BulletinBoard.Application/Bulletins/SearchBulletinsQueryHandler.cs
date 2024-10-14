using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins;

public class SearchBulletinsQueryHandler : IRequestHandler<ISearchBulletinsQuery, Bulletin[]>
{
    private readonly IBulletinRepository _bulletins;

    public SearchBulletinsQueryHandler(IBulletinRepository bulletins)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
    }

    public async Task<Bulletin[]> Handle(ISearchBulletinsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _bulletins.SearchAsync(
            request.Page,
            request.Number,
            request.Text,
            request.UserId,
            request.SortBy,
            request.Desc,
            request.Created,
            request.Expiry,
            cancellationToken);
    }
}