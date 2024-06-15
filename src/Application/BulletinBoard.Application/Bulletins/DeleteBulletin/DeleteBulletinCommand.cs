using Ardalis.GuardClauses;
using MediatR;

namespace BulletinBoard.Application.Bulletins.DeleteBulletin;

public record DeleteBulletinCommand : IRequest
{
    public DeleteBulletinCommand(Guid id)
    {
        Guard.Against.Default(
            id,
            nameof(id),
            "Не может иметь значение по умолчанию.");

        Id = id;
    }

    public Guid Id { get; }
}