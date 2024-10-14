using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Commands;

public interface ICreateUserCommand : IRequest<Guid>
{
    string Name { get; }
    bool IsAdmin { get; }
}