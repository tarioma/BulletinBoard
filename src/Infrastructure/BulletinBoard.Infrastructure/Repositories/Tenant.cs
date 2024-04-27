using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Exceptions;

namespace BulletinBoard.Infrastructure.Repositories;

public class Tenant : ITenant
{
    private readonly DatabaseContext _context;

    public Tenant(DatabaseContext context)
    {
        Guard.Against.Null(context);

        _context = context;

        Users = new UserRepository(context);
        Bulletins = new BulletinRepository(context);
    }

    public IBulletinRepository Bulletins { get; }
    public IUserRepository Users { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}