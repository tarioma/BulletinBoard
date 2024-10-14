using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface IGetBulletinByIdQuery : IRequest<Bulletin>
{
    Guid Id { get; }
}