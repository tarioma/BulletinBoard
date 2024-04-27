using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.CreateUser;

public class CreateUserCommandHandler : BaseHandler, IRequestHandler<CreateUserCommand, Guid>
{
    public CreateUserCommandHandler(ITenantFactory tenantFactory) : base(tenantFactory) { }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var user = User.Create(request.Name, request.IsAdmin);
        var tenant = TenantFactory.GetTenant();

        await tenant.Users.CreateAsync(user, cancellationToken);
        await tenant.CommitAsync(cancellationToken);

        return user.Id;
    }
}