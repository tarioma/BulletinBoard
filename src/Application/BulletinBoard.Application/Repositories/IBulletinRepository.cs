using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.Specifications;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Repositories;

public interface IBulletinRepository
{
    Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken);
    Task<Bulletin> GetByIdAsync(ISpecification<Bulletin> specification, CancellationToken cancellationToken);
    Task<IEnumerable<Bulletin>> SearchAsync(BulletinsSearchFilters searchFilters, CancellationToken cancellationToken);
    Task<int> GetUserBulletinsCountAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}