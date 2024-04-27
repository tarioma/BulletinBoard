using Ardalis.GuardClauses;
using BulletinBoard.Infrastructure.Context;

namespace BulletinBoard.Infrastructure.Repositories.Common;

public class BaseRepository
{
    protected readonly DatabaseContext Context;

    public BaseRepository(DatabaseContext context)
    {
        Guard.Against.Null(context);

        Context = context;
    }
}