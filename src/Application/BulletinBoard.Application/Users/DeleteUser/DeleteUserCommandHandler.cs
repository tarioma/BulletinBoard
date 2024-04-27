using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using MediatR;

namespace BulletinBoard.Application.Users.DeleteUser;

public class DeleteUserCommandHandler : BaseHandler, IRequestHandler<DeleteUserCommand>
{
    public DeleteUserCommandHandler(ITenantFactory tenantFactory) : base(tenantFactory) { }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        await tenant.Users.DeleteAsync(request.Id, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
    }
}