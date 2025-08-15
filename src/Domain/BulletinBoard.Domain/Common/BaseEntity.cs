using Ardalis.GuardClauses;

namespace BulletinBoard.Domain.Common;

public abstract class BaseEntity
{
    internal BaseEntity(Guid id, DateTime createdUtc)
    {
        Guard.Against.Default(id);
        Guard.Against.Default(createdUtc);

        Id = id;
        CreatedUtc = createdUtc;
    }

    protected BaseEntity()
    {
    }

    public Guid Id { get; }
    public DateTime CreatedUtc { get; }
}