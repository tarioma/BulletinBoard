using MediatR;

namespace BulletinBoard.Application.Users.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest;