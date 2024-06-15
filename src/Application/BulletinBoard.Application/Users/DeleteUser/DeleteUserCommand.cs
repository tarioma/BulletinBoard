using Ardalis.GuardClauses;
using MediatR;

namespace BulletinBoard.Application.Users.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public DeleteUserCommand(Guid id)
    {
        Guard.Against.Default(
            id,
            nameof(id),
            "Не может иметь значение по умолчанию.");

        Id = id;
    }

    public Guid Id { get; }
}