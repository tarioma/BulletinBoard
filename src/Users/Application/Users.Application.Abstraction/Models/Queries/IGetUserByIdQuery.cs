using MediatR;
using Users.Domain.Entities;

namespace Users.Application.Abstraction.Models.Queries;

public interface IGetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; }
}