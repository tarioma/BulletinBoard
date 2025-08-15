using Ardalis.GuardClauses;
using BulletinBoard.Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Infrastructure.Specifications;

internal static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T> spec) where T : class
    {
        Guard.Against.Null(input);
        Guard.Against.Null(spec);

        var query = input.AsNoTracking();

        query = query.Where(spec.Criteria);

        return query;
    }
}