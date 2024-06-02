using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Exceptions;
using BulletinBoard.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.Infrastructure.Repositories;

public class Tenant : ITenant
{
    private readonly DatabaseContext _context;

    public Tenant(DatabaseContext context, IServiceProvider serviceProvider)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(serviceProvider);
        Guard.Against.MissingService<IBulletinRepository>(serviceProvider);
        Guard.Against.MissingService<IUserRepository>(serviceProvider);

        _context = context;

        Bulletins = serviceProvider.GetService<IBulletinRepository>()!;
        Users = serviceProvider.GetService<IUserRepository>()!;
    }

    public IBulletinRepository Bulletins { get; }
    public IUserRepository Users { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}