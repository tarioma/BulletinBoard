using MediatR;

namespace BulletinBoard.Application.Users.CreateUser;

public record CreateUserCommand(string Name, bool IsAdmin) : IRequest<Guid>;