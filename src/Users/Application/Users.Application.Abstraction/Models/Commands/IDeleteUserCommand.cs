using MediatR;

namespace Users.Application.Abstraction.Models.Commands;

public interface IDeleteUserCommand : IRequest
{
    Guid Id { get; }
}