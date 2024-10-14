using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BulletinBoard.Application.Users;

public class CreateUserCommandHandler : IRequestHandler<ICreateUserCommand, Guid>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository users, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Guid> Handle(ICreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = _mapper.Map<User>(request);

        await _users.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}