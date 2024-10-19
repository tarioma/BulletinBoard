using Microsoft.EntityFrameworkCore;
using Users.Application.Abstraction.Repositories;
using Users.Dal.Exceptions;
using Users.Domain.Entities;

namespace Users.Dal.Repositories;

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

    public async Task<User[]> SearchAsync(int page, int pageSize, string? text, bool? isAdmin, string? sortBy, bool desc,
        DateTime? createdFrom, DateTime? createdTo, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsQueryable()
            .AsNoTracking()
            .Where(u => createdFrom == null || u.CreatedUtc >= createdFrom)
            .Where(u => createdTo == null || u.CreatedUtc <= createdTo)
            .Where(u => text == null || EF.Functions.ILike(u.Name, $"%{text.Trim()}%"))
            .Where(u => isAdmin == null || u.IsAdmin == isAdmin)
            .OrderBy(u => sortBy == null || desc ? $"{sortBy} descending" : sortBy)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Update(user);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Не может быть пустым uuid.", nameof(id));
        }

        await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}