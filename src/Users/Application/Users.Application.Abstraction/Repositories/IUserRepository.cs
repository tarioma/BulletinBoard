using Users.Domain.Entities;

namespace Users.Application.Abstraction.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User[]> SearchAsync(int page, int pageSize, string? text, bool? isAdmin, string? sortBy, bool desc, DateTime? createdFrom,
        DateTime? createdTo, CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}