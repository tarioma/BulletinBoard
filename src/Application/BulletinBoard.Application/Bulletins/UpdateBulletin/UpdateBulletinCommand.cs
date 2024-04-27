using MediatR;

namespace BulletinBoard.Application.Bulletins.UpdateBulletin;

public record UpdateBulletinCommand(
    Guid Id,
    string Text,
    int Rating,
    DateTime ExpiryUtc,
    Stream? ImageStream,
    string? ImageExtension) : IRequest;