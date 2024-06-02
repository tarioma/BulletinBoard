using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Infrastructure.Context;

namespace BulletinBoard.Infrastructure.Repositories;

public class TenantFactory : ITenantFactory
{
    private readonly Lazy<ITenant> _tenant;

    public TenantFactory(DatabaseContext context, IServiceProvider serviceProvider)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(serviceProvider);

        _tenant = new Lazy<ITenant>(() => new Tenant(context, serviceProvider));
    }

    public ITenant GetTenant() => _tenant.Value;
}