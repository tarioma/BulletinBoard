using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;

namespace BulletinBoard.Application.Common;

public abstract class BaseHandler
{
    protected readonly ITenantFactory TenantFactory;

    protected BaseHandler(ITenantFactory tenantFactory)
    {
        Guard.Against.Null(tenantFactory);

        TenantFactory = tenantFactory;
    }
}