using BulletinBoard.Application.Abstraction.Models.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Abstraction.Repositories;

public interface IBulletinRepository
{
    Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken = default);

    Task<Bulletin> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Bulletin[]> SearchAsync(
        PageFilter page, int? number, string? text, Guid? userId, string? sortBy, bool desc,
        DateRangeFilters created, DateRangeFilters expiry, CancellationToken cancellationToken = default);

    Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}