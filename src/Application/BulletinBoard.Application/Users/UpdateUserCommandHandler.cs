using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Repositories;
using MapsterMapper;
using MediatR;

namespace BulletinBoard.Application.Users;

public class UpdateUserCommandHandler : IRequestHandler<IUpdateUserCommand>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository users, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(IUpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _users.GetByIdAsync(request.Id, cancellationToken);
        _mapper.Map(user, request);

        await _users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}