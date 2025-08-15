using System.Linq.Dynamic.Core;
using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using NotFoundException = BulletinBoard.Infrastructure.Exceptions.NotFoundException;

namespace BulletinBoard.Infrastructure.Repositories;

public class UserRepository(DatabaseContext context) : IUserRepository
{
    public Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(user);

        return Task.FromResult(context.Users.Add(user));
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        return await context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
               ?? throw new NotFoundException("Пользователь с таким id не найден.");
    }

    public async Task<IEnumerable<User>> SearchAsync(
        UsersSearchFilters searchFilters,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(searchFilters);

        var users = context.Users.AsQueryable().AsNoTracking();

        if (searchFilters.Created.To is not null)
        {
            users = users.Where(u => u.CreatedUtc >= searchFilters.Created.From);
        }

        if (searchFilters.Created.To is not null)
        {
            users = users.Where(u => u.CreatedUtc <= searchFilters.Created.To);
        }

        if (searchFilters.SearchName is not null)
        {
            var text = searchFilters.SearchName.Trim();
            users = users.Where(u => EF.Functions.ILike(u.Name, $"%{text}%"));
        }

        if (searchFilters.SearchIsAdmin is not null)
        {
            users = users.Where(u => u.IsAdmin == searchFilters.SearchIsAdmin);
        }

        users = searchFilters.Desc
            ? users.OrderBy($"{searchFilters.SortBy} descending")
            : users.OrderBy(searchFilters.SortBy);
        
        return await users
            .Skip(searchFilters.Page.Offset)
            .Take(searchFilters.Page.Count)
            .ToArrayAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(user);

        context.Update(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        context.Users.Where(u => u.Id == id).ExecuteDelete();

        return Task.CompletedTask;
    }
}