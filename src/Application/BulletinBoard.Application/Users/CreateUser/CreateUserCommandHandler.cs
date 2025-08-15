using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.CreateUser;

public class CreateUserCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var user = User.Create(request.Name, request.IsAdmin);

        await users.CreateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}