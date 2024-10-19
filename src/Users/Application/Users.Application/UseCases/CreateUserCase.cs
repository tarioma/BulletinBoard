using AutoMapper;
using MediatR;
using Users.Application.Abstraction.Models.Commands;
using Users.Application.Abstraction.Repositories;
using Users.Domain.Entities;

namespace Users.Application.UseCases;

public class CreateUserCase : IRequestHandler<ICreateUserCommand, Guid>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCase(IUserRepository users, IUnitOfWork unitOfWork, IMapper mapper)
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