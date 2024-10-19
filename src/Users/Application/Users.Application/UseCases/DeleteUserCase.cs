using MediatR;
using Users.Application.Abstraction.Models.Commands;
using Users.Application.Abstraction.Repositories;

namespace Users.Application.UseCases;

public class DeleteUserCase : IRequestHandler<IDeleteUserCommand>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCase(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(IDeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _users.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}