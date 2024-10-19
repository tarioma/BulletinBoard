using Bulletins.Domain.Entities;
using MediatR;

namespace Bulletins.Application.Abstraction.Models.Queries;

public interface IGetBulletinByIdQuery : IRequest<Bulletin>
{
    Guid Id { get; }
}