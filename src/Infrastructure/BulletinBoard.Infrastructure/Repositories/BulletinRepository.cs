using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Exceptions;
using BulletinBoard.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using NotFoundException = BulletinBoard.Infrastructure.Exceptions.NotFoundException;

namespace BulletinBoard.Infrastructure.Repositories;

public class BulletinRepository : BaseRepository, IBulletinRepository
{
    public BulletinRepository(DatabaseContext context) : base(context)
    {
    }

    public Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(bulletin);

        return Task.FromResult(Context.Bulletins.Add(bulletin));
    }

    public async Task<Bulletin> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        return await Context.Bulletins.SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
               ?? throw new NotFoundException("Объявление с таким id не найдено.");
    }

    public async Task<IEnumerable<Bulletin>> SearchAsync(
        BulletinsSearchFilters searchFilters,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(searchFilters);

        var bulletins = Context.Bulletins.AsQueryable();

        if (searchFilters.RatingFrom is not null)
        {
            bulletins = bulletins.Where(b => b.Rating >= searchFilters.RatingFrom);
        }

        if (searchFilters.RatingTo is not null)
        {
            bulletins = bulletins.Where(b => b.Rating <= searchFilters.RatingTo);
        }

        if (searchFilters.CreatedFromUtc is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc >= searchFilters.CreatedFromUtc);
        }

        if (searchFilters.CreatedToUtc is not null)
        {
            bulletins = bulletins.Where(b => b.CreatedUtc <= searchFilters.CreatedToUtc);
        }

        if (searchFilters.ExpiryFromUtc is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc >= searchFilters.ExpiryFromUtc);
        }

        if (searchFilters.ExpiryToUtc is not null)
        {
            bulletins = bulletins.Where(b => b.ExpiryUtc <= searchFilters.ExpiryToUtc);
        }

        if (searchFilters.SearchNumber is not null)
        {
            bulletins = bulletins.Where(b =>
                EF.Functions.ILike(b.Number.ToString(), $"%{searchFilters.SearchNumber}%"));
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

        bulletins = searchFilters.SortBy switch
        {
            BulletinsSortBy.Created when searchFilters.Desc => bulletins.OrderByDescending(b => b.CreatedUtc),
            BulletinsSortBy.Created => bulletins.OrderBy(b => b.CreatedUtc),
            BulletinsSortBy.Number when searchFilters.Desc => bulletins.OrderByDescending(b => b.Number),
            BulletinsSortBy.Number => bulletins.OrderBy(b => b.Number),
            BulletinsSortBy.Text when searchFilters.Desc => bulletins.OrderByDescending(b => b.Text),
            BulletinsSortBy.Text => bulletins.OrderBy(b => b.Text),
            BulletinsSortBy.Rating when searchFilters.Desc => bulletins.OrderByDescending(b => b.Rating),
            BulletinsSortBy.Rating => bulletins.OrderBy(b => b.Rating),
            _ => bulletins
        };

        return await bulletins
            .Skip(searchFilters.Page * searchFilters.Count)
            .Take(searchFilters.Count)
            .ToArrayAsync(cancellationToken);
    }

    public Task<int> GetUserBulletinsCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        Guard.Against.Default(userId);

        return Context.Bulletins.Where(b => b.UserId == userId).CountAsync(cancellationToken);
    }

    public Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(bulletin);

        Context.Update(bulletin);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Guard.Against.Default(id);

        var bulletin = await GetByIdAsync(id, cancellationToken);
        Context.Remove(bulletin);
    }
}