using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Infrastructure.Context;

namespace BulletinBoard.Infrastructure.Repositories;

public class TenantFactory : ITenantFactory
{
    private readonly Lazy<ITenant> _tenant;

    public TenantFactory(DatabaseContext context)
    {
        Guard.Against.Null(context);

        _tenant = new Lazy<ITenant>(new Tenant(context));
    }

    public ITenant GetTenant() => _tenant.Value;
}