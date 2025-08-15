using System.Linq.Expressions;

namespace BulletinBoard.Application.Specifications;

public class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
    }

    public Expression<Func<T, bool>> Criteria { get; }
}