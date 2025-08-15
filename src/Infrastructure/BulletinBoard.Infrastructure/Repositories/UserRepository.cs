using Ardalis.GuardClauses;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Specifications;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Infrastructure.Context;
using BulletinBoard.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            Guard.Against.Null(user);
            await _db.Users.AddAsync(user, cancellationToken);
        }

        public async Task<User> GetByIdAsync(ISpecification<User> specification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(specification);

            // Используем общий Evaluator для применения спецификации
            var query = SpecificationEvaluator.GetQuery(_db.Users, specification);

            // В интерфейсе тип "User" не допускает null — ожидаем единственную запись.
            return await query.SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> SearchAsync(UsersSearchFilters filters,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(filters);

            IQueryable<User> query = _db.Users.AsNoTracking();

            // Фильтр по имени (case-insensitive, постгрес — через ILike; для других провайдеров .ToLower())
            if (!string.IsNullOrWhiteSpace(filters.SearchName))
            {
                var pattern = $"%{filters.SearchName.Trim()}%";

                // Попытка транслировать в ILIKE (работает в Npgsql)
                if (EF.Functions != null)
                {
                    query = query.Where(u => EF.Functions.ILike(u.Name, pattern));
                }
                else
                {
                    var name = filters.SearchName.Trim().ToLower();
                    query = query.Where(u => u.Name.ToLower().Contains(name));
                }
            }

            // Фильтр по IsAdmin
            if (filters.SearchIsAdmin.HasValue)
            {
                var isAdmin = filters.SearchIsAdmin.Value;
                query = query.Where(u => u.IsAdmin == isAdmin);
            }

            // Фильтр по диапазону дат создания
            if (filters.Created.From.HasValue)
            {
                var from = filters.Created.From.Value;
                query = query.Where(u => u.CreatedUtc >= from);
            }

            if (filters.Created.To.HasValue)
            {
                var to = filters.Created.To.Value;
                query = query.Where(u => u.CreatedUtc <= to);
            }

            // Сортировка
            query = ApplySorting(query, filters.SortBy, filters.Desc);

            // Пагинация
            query = query
                .Skip(filters.Page.Offset)
                .Take(filters.Page.Count);

            return await query.ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            Guard.Against.Null(user);
            _db.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Guard.Against.Default(id);

            // Без лишней загрузки — можно использовать ExecuteDeleteAsync (EF Core 7+),
            // но для совместимости удалим через FindAsync.
            var entity = await _db.Users.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Пользователь с идентификатором '{id}' не найден.");
            }

            _db.Users.Remove(entity);
        }

        private static IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, bool desc)
        {
            // Значения приходят из UsersSearchFilters.SortOptions:
            // CreatedUtc, Name, IsAdmin
            return sortBy switch
            {
                nameof(User.Name) => desc
                    ? query.OrderByDescending(u => u.Name).ThenByDescending(u => u.CreatedUtc)
                    : query.OrderBy(u => u.Name).ThenBy(u => u.CreatedUtc),

                nameof(User.IsAdmin) => desc
                    ? query.OrderByDescending(u => u.IsAdmin).ThenByDescending(u => u.CreatedUtc)
                    : query.OrderBy(u => u.IsAdmin).ThenBy(u => u.CreatedUtc),

                _ => desc
                    ? query.OrderByDescending(u => u.CreatedUtc)
                    : query.OrderBy(u => u.CreatedUtc),
            };
        }
    }
}