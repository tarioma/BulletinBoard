using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Specifications;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.GetBulletinById;

public class GetBulletinByIdQueryHandler(
    IBulletinRepository bulletins)
    : IRequestHandler<GetBulletinByIdQuery, Bulletin>
{
    public async Task<Bulletin> Handle(GetBulletinByIdQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        return await bulletins.GetByIdAsync(
            new BulletinByIdSpecification(request.Id),
            cancellationToken);
    }
}