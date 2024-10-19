using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Dal.Exceptions;
using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Dal.Repositories;

public class BulletinRepository : IBulletinRepository
{
    private readonly DatabaseContext _context;

    public BulletinRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bulletin);

        return Task.FromResult(_context.Bulletins.Add(bulletin));
    }

    public async Task<Bulletin> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Не может быть пустым uuid.", nameof(id));
        }

        return await _context.Bulletins.AsNoTracking().SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
               ?? throw new NotFoundException("Объявление с таким id не найдено.");
    }

    public async Task<Bulletin[]> SearchAsync(int page, int pageSize, int? number, string? text, Guid? userId, string? sortBy, bool desc,
        DateTime? createdFrom, DateTime? createdTo, DateTime? expiryFrom, DateTime? expiryTo, CancellationToken cancellationToken)
    {
        return await _context.Bulletins
            .AsQueryable()
            .AsNoTracking()
            .Where(b => createdFrom == null || b.CreatedUtc >= createdFrom)
            .Where(b => createdTo == null || b.CreatedUtc <= createdTo)
            .Where(b => expiryFrom == null || b.ExpiryUtc >= expiryFrom)
            .Where(b => expiryTo == null || b.ExpiryUtc <= expiryTo)
            .Where(b => number == null || b.Number == number)
            .Where(b => text == null || EF.Functions.ILike(b.Text, $"%{text.Trim()}%"))
            .Where(b => userId == null || b.UserId == userId)
            .OrderBy(u => sortBy == null || desc ? $"{sortBy} descending" : sortBy)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bulletin);

        _context.Update(bulletin);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Не может быть пустым uuid.", nameof(id));
        }

        await _context.Bulletins.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}