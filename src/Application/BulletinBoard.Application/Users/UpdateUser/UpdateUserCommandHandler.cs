using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using MediatR;

namespace BulletinBoard.Application.Users.UpdateUser;

public class UpdateUserCommandHandler : BaseHandler, IRequestHandler<UpdateUserCommand>
{
    public UpdateUserCommandHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        var user = await tenant.Users.GetByIdAsync(request.Id, cancellationToken);
        user.SetName(request.Name);
        user.IsAdmin = request.IsAdmin;

        await tenant.Users.UpdateAsync(user, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
    }
}