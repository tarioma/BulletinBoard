using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.SearchBulletins;

public class SearchBulletinsQueryHandler : BaseHandler, IRequestHandler<SearchBulletinsQuery, IEnumerable<Bulletin>>
{
    public SearchBulletinsQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }

    public async Task<IEnumerable<Bulletin>> Handle(
        SearchBulletinsQuery request,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        return await tenant.Bulletins.SearchAsync(request.SearchFilters, cancellationToken);
    }
}