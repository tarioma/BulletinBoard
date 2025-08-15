using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Specifications;
using MediatR;

namespace BulletinBoard.Application.Users.UpdateUser;

public class UpdateUserCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var user = await users.GetByIdAsync(
            new UserByIdSpecification(request.Id),
            cancellationToken);

        user.SetName(request.Name);
        user.IsAdmin = request.IsAdmin;

        await users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}