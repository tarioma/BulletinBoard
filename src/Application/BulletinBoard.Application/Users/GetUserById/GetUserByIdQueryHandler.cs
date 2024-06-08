using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.GetUserById;

public class GetUserByIdQueryHandler : BaseHandler, IRequestHandler<GetUserByIdQuery, User>
{
    public GetUserByIdQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        return await tenant.Users.GetByIdAsync(request.Id, cancellationToken);
    }
}