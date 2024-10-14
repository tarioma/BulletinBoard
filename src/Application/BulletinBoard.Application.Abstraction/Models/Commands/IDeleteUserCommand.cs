using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Commands;

public interface IDeleteUserCommand : IRequest
{
    Guid Id { get; }
}