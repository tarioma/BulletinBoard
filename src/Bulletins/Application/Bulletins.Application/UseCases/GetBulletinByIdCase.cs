using Bulletins.Application.Abstraction.Models.Queries;
using Bulletins.Application.Abstraction.Repositories;
using Bulletins.Domain.Entities;
using MediatR;

namespace Bulletins.Application.UseCases;

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