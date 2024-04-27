using MediatR;

namespace BulletinBoard.Application.Bulletins.DeleteBulletin;

public record DeleteBulletinCommand(Guid Id) : IRequest;