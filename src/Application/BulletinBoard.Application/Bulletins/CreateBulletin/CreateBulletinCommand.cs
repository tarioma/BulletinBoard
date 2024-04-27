using MediatR;

namespace BulletinBoard.Application.Bulletins.CreateBulletin;

public record CreateBulletinCommand(
    string Text,
    int Rating,
    DateTime ExpiryUtc,
    Guid UserId,
    Stream? ImageStream,
    string? ImageExtension) : IRequest<Guid>;