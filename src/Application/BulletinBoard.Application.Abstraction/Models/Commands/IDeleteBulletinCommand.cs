using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Commands;

public interface IDeleteBulletinCommand : IRequest
{
    Guid Id { get; }
}