using System.Linq.Dynamic.Core;
using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using NotFoundException = BulletinBoard.Infrastructure.Exceptions.NotFoundException;

namespace BulletinBoard.Infrastructure.Repositories;

public class BulletinRepository(DatabaseContext context) : IBulletinRepository
{
    public Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(bulletin);

        return Task.FromResult(context.Bulletins.Add(bulletin));
    }

    public async Task<Bulletin> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        return await context.Bulletins.AsNoTracking().SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
               ?? throw new NotFoundException("Объявление с таким id не найдено.");
    }

    public async Task<IEnumerable<Bulletin>> SearchAsync(
        BulletinsSearchFilters searchFilters,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(searchFilters);

        var bulletins = context.Bulletins.AsQueryable().AsNoTracking();

        if (searchFilters.Rating.From is not null)
        {
            bulletins = bulletins.Where(b => b.Rating >= searchFilters.Rating.From);
        }

        if (searchFilters.Rating.To is not null)
        {
            bulletins = bulletins.Where(b => b.Rating <= searchFilters.Rating.To);
        }

        if (searchFilters.Created.From is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc >= searchFilters.Created.From);
        }

        if (searchFilters.Created.To is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc <= searchFilters.Created.To);
        }

        if (searchFilters.Expiry.From is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc >= searchFilters.Expiry.From);
        }

        if (searchFilters.Expiry.To is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc <= searchFilters.Expiry.To);
        }

        if (searchFilters.SearchNumber is not null)
        {
            bulletins = bulletins.Where(b => b.Number == searchFilters.SearchNumber);
        }

        if (searchFilters.SearchText is not null)
        {
            var text = searchFilters.SearchText.Trim();
            bulletins = bulletins.Where(b => EF.Functions.ILike(b.Text, $"%{text}%"));
        }

        if (searchFilters.SearchUserId is not null)
        {
            bulletins = bulletins.Where(b => b.UserId == searchFilters.SearchUserId);
        }

        bulletins = searchFilters.Desc
            ? bulletins.OrderBy($"{searchFilters.SortBy} descending")
            : bulletins.OrderBy(searchFilters.SortBy);

        return await bulletins
            .Skip(searchFilters.Page.Offset)
            .Take(searchFilters.Page.Count)
            .ToArrayAsync(cancellationToken);
    }

    public Task<int> GetUserBulletinsCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        Guard.Against.Default(userId);

        return context.Bulletins.Where(b => b.UserId == userId).CountAsync(cancellationToken);
    }

    public Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(bulletin);

        context.Update(bulletin);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        context.Bulletins.Where(u => u.Id == id).ExecuteDelete();

        return Task.CompletedTask;
    }
}