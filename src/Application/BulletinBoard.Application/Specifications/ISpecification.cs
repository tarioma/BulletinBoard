using System.Linq.Expressions;

namespace BulletinBoard.Application.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}