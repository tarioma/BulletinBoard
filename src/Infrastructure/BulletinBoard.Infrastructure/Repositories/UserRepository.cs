using System.Linq.Dynamic.Core;
using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using NotFoundException = BulletinBoard.Infrastructure.Exceptions.NotFoundException;

namespace BulletinBoard.Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(DatabaseContext context) : base(context)
    {
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(user);

        return Task.FromResult(Context.Users.Add(user));
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        return await Context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
               ?? throw new NotFoundException("Пользователь с таким id не найден.");
    }

    public async Task<IEnumerable<User>> SearchAsync(
        UsersSearchFilters searchFilters,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(searchFilters);

        var users = Context.Users.AsQueryable().AsNoTracking();

        if (searchFilters.Created.From is not null)
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

        var sortOptions = new Dictionary<string, string>
        {
            { "created", nameof(User.CreatedUtc) },
            { "name", nameof(User.Name) },
            { "isadmin", nameof(User.IsAdmin) }
        };
        var sortBy = sortOptions.GetValueOrDefault(
            searchFilters.SortBy?.ToLower() ?? "created", nameof(User.CreatedUtc));
        users = searchFilters.Desc
            ? users.OrderBy($"{sortBy} descending")
            : users.OrderBy(sortBy);

        return await users
            .Skip(searchFilters.Page.Offset)
            .Take(searchFilters.Page.Count)
            .ToArrayAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(user);

        Context.Update(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        Context.Users.Where(u => u.Id == id).ExecuteDelete();

        return Task.CompletedTask;
    }
}