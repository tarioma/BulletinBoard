using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Commands;

public interface ICreateBulletinCommand : IRequest<Guid>
{
    string Text { get; }
    DateTime ExpiryUtc { get; }
    string? Image { get; }
    Guid UserId { get; }
}