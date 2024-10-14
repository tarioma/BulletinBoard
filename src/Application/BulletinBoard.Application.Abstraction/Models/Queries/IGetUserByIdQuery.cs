using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Abstraction.Models.Queries;

public interface IGetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; }
}