using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<User>;