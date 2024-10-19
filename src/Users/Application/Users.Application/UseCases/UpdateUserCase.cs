using AutoMapper;
using MediatR;
using Users.Application.Abstraction.Models.Commands;
using Users.Application.Abstraction.Repositories;

namespace Users.Application.UseCases;

public class UpdateUserCase : IRequestHandler<IUpdateUserCommand>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCase(IUserRepository users, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(IUpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _users.GetByIdAsync(request.Id, cancellationToken);
        _mapper.Map(request, user);

        await _users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}