using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.GetUserById;

public class GetUserByIdQuery : IRequest<User>
{
    public GetUserByIdQuery(Guid id)
    {
        Guard.Against.Default(
            id,
            nameof(id),
            "Не может иметь значение по умолчанию.");

        Id = id;
    }

    public Guid Id { get; }
}