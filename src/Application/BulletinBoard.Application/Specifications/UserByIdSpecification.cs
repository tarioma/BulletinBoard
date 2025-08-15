using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Specifications;

public sealed class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(Guid id) 
        : base(u => u.Id == id)
    {
        Guard.Against.Default(id);
    }
}