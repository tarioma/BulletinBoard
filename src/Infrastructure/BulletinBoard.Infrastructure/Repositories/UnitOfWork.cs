using BulletinBoard.Application.Repositories;
using BulletinBoard.Infrastructure.Context;

namespace BulletinBoard.Infrastructure.Repositories;

public class UnitOfWork(DatabaseContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}