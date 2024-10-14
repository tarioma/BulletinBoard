using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Dal.Exceptions;
using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using BulletinBoard.Application.Abstraction.Models.SearchFilters;

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

    public async Task<Bulletin[]> SearchAsync(PageFilter page, int? number, string? text, Guid? userId, string? sortBy, bool desc,
        DateRangeFilters created, DateRangeFilters expiry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(created);
        ArgumentNullException.ThrowIfNull(expiry);

        var bulletins = _context.Bulletins.AsQueryable().AsNoTracking();

        if (created.From is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc >= created.From);
        }

        if (created.To is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc <= created.To);
        }

        if (expiry.From is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc >= expiry.From);
        }

        if (expiry.To is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc <= expiry.To);
        }

        if (number is not null)
        {
            bulletins = bulletins.Where(b => b.Number == number);
        }

        if (text is not null)
        {
            bulletins = bulletins.Where(b => EF.Functions.ILike(b.Text, $"%{text.Trim()}%"));
        }

        if (userId is not null)
        {
            bulletins = bulletins.Where(b => b.UserId == userId);
        }

        if (sortBy is not null)
        {
            bulletins = desc
                ? bulletins.OrderBy($"{sortBy} descending")
                : bulletins.OrderBy(sortBy);
        }

        return await bulletins
            .Skip(page.Offset)
            .Take(page.Count)
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