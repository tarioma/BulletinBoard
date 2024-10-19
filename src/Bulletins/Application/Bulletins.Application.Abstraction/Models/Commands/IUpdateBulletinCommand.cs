using MediatR;

namespace Bulletins.Application.Abstraction.Models.Commands;

public interface IUpdateBulletinCommand : IRequest
{
    Guid Id { get; }
    string Text { get; }
    DateTime ExpiryUtc { get; }
    string? Image { get; }
}