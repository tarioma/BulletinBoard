﻿using System.Reflection;
using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Infrastructure.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Guard.Against.Null(options);

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Bulletin> Bulletins => Set<Bulletin>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}