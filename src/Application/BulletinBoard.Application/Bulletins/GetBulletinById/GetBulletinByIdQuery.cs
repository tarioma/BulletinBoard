using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.GetBulletinById;

public record GetBulletinByIdQuery(Guid Id) : IRequest<Bulletin>;