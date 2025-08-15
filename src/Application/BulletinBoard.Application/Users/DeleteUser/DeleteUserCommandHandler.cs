using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using MediatR;

namespace BulletinBoard.Application.Users.DeleteUser;

public class DeleteUserCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        await users.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}