using MediatR;

namespace BulletinBoard.Application.Users.UpdateUser;

public record UpdateUserCommand(Guid Id, string Name, bool IsAdmin) : IRequest;