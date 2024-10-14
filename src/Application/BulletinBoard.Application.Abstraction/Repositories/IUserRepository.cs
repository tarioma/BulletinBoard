using BulletinBoard.Application.Abstraction.Models.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Abstraction.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User[]> SearchAsync(PageFilter page, string? text, bool? isAdmin, string? sortBy, bool desc, DateRangeFilters created,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}