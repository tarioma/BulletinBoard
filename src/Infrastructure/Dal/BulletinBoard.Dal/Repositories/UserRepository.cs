using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using BulletinBoard.Application.Abstraction.Models.SearchFilters;
using BulletinBoard.Dal.Exceptions;

namespace BulletinBoard.Dal.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        return Task.FromResult(_context.Users.Add(user));
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Не может быть пустым uuid.", nameof(id));
        }

        return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
               ?? throw new NotFoundException("Пользователь с таким id не найден.");
    }

    public async Task<User[]> SearchAsync(PageFilter page, string? text, bool? isAdmin, string? sortBy, bool desc,
        DateRangeFilters created, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(created);

        var users = _context.Users.AsQueryable().AsNoTracking();

        if (created.To is not null)
        {
            users = users.Where(u => u.CreatedUtc >= created.From);
        }

        if (created.To is not null)
        {
            users = users.Where(u => u.CreatedUtc <= created.To);
        }

        if (text is not null)
        {
            users = users.Where(u => EF.Functions.ILike(u.Name, $"%{text.Trim()}%"));
        }

        if (isAdmin is not null)
        {
            users = users.Where(u => u.IsAdmin == isAdmin);
        }

        if (sortBy is not null)
        {
            users = desc
                ? users.OrderBy($"{sortBy} descending")
                : users.OrderBy(sortBy);
        }

        return await users
            .Skip(page.Offset)
            .Take(page.Count)
            .ToArrayAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Update(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Не может быть пустым uuid.", nameof(id));
        }

        _context.Users.Where(u => u.Id == id).ExecuteDelete();

        return Task.CompletedTask;
    }
}