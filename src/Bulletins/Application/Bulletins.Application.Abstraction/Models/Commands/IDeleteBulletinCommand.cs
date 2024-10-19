using MediatR;

namespace Bulletins.Application.Abstraction.Models.Commands;

public interface IDeleteBulletinCommand : IRequest
{
    Guid Id { get; }
}