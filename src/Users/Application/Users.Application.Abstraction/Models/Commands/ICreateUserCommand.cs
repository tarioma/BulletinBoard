using MediatR;

namespace Users.Application.Abstraction.Models.Commands;

public interface ICreateUserCommand : IRequest<Guid>
{
    string Name { get; }
    bool IsAdmin { get; }
}