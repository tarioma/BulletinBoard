using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Commands;

public interface IUpdateUserCommand : IRequest
{
    Guid Id { get; }
    string Name { get; }
    bool IsAdmin { get; }
}