using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Specifications;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task<User> GetByIdAsync(ISpecification<User> specification, CancellationToken cancellationToken);
    Task<IEnumerable<User>> SearchAsync(UsersSearchFilters filters, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}