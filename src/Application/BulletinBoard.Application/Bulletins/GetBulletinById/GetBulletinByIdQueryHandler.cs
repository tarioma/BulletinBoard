using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.GetBulletinById;

public class GetBulletinByIdQueryHandler : BaseHandler, IRequestHandler<GetBulletinByIdQuery, Bulletin>
{
    public GetBulletinByIdQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory) { }

    public async Task<Bulletin> Handle(GetBulletinByIdQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        return await tenant.Bulletins.GetByIdAsync(request.Id, cancellationToken);
    }
}