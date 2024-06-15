using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.GetBulletinById;

public record GetBulletinByIdQuery : IRequest<Bulletin>
{
    public GetBulletinByIdQuery(Guid id)
    {
        Guard.Against.Default(
            id,
            nameof(id),
            "Не может иметь значение по умолчанию.");

        Id = id;
    }

    public Guid Id { get; }
}