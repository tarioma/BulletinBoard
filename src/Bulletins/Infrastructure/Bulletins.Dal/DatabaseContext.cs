using System.Reflection;
using Bulletins.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bulletins.Dal;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Bulletin> Bulletins => Set<Bulletin>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
