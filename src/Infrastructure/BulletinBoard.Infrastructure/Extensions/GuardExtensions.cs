using Ardalis.GuardClauses;
using BulletinBoard.Infrastructure.Exceptions;

namespace BulletinBoard.Infrastructure.Extensions;

public static class GuardExtensions
{
    public static void MissingService<T>(this IGuardClause _, IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(T)) is null)
        {
            throw new MissingServiceException($"Отсутствует необходимый сервис {typeof(T).Name}.");
        }
    }
}