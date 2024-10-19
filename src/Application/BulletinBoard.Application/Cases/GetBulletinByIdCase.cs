using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class GetBulletinByIdCase : IRequestHandler<IGetBulletinByIdQuery, Bulletin>
{
    private readonly IBulletinRepository _bulletins;

    public GetBulletinByIdCase(IBulletinRepository bulletins)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
    }

    public async Task<Bulletin> Handle(IGetBulletinByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _bulletins.GetByIdAsync(request.Id, cancellationToken);
    }
}