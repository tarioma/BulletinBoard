using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Specifications;

public sealed class BulletinByIdSpecification : Specification<Bulletin>
{
    public BulletinByIdSpecification(Guid id) 
        : base(u => u.Id == id)
    {
        Guard.Against.Default(id);
    }
}