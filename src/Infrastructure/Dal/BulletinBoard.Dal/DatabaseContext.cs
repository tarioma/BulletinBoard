using System.Reflection;
using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Dal;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Bulletin> Bulletins => Set<Bulletin>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
