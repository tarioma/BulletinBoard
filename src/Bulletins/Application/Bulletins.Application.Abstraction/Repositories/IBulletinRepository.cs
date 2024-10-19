using Bulletins.Domain.Entities;

namespace Bulletins.Application.Abstraction.Repositories;

public interface IBulletinRepository
{
    Task CreateAsync(Bulletin bulletin, CancellationToken cancellationToken = default);

    Task<Bulletin> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Bulletin[]> SearchAsync(int page, int pageSize, int? number, string? text, Guid? userId, string? sortBy, bool desc,
        DateTime? createdFrom, DateTime? createdTo, DateTime? expiryFrom, DateTime? expiryTo,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(Bulletin bulletin, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}