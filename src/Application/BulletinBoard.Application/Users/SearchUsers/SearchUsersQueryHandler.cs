using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.SearchUsers;

public class SearchUsersQueryHandler : BaseHandler, IRequestHandler<SearchUsersQuery, IEnumerable<User>>
{
    public SearchUsersQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory) { }

    public async Task<IEnumerable<User>> Handle(SearchUsersQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        return await tenant.Users.SearchAsync(request.SearchFilters, cancellationToken);
    }
}