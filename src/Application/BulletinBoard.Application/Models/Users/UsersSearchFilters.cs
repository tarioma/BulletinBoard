using Ardalis.GuardClauses;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Models.Users;

public class UsersSearchFilters
{
    private const string DefaultSortOption = nameof(User.CreatedUtc);

    private static readonly string[] SortOptions = [nameof(User.CreatedUtc), nameof(User.Name), nameof(User.IsAdmin)];

    public UsersSearchFilters(
        PageFilter page,
        string? searchName,
        bool? searchIsAdmin,
        string? sortBy,
        bool desc,
        DateRangeFilters created)
    {
        if (searchName is not null)
        {
            Guard.Against.WhiteSpace(
                searchName,
                nameof(searchName),
                "Не может быть пустой строкой.");

            Guard.Against.StringTooLong(
                searchName,
                User.MaxNameLength,
                nameof(searchName),
                $"Максимальная длина: {nameof(User.MaxNameLength)}.");
        }

        if (sortBy is not null && !SortOptions.Any(s => s.Equals(sortBy, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new ArgumentException(
                $"Неверный параметр сортировки. Возможные варианты: {string.Join(", ", SortOptions)}.", nameof(sortBy));
        }

        Page = page;
        SearchName = searchName;
        SearchIsAdmin = searchIsAdmin;
        SortBy = sortBy ?? DefaultSortOption;
        Desc = desc;
        Created = created;
    }

    public PageFilter Page { get; }
    public string? SearchName { get; }
    public bool? SearchIsAdmin { get; }
    public string SortBy { get; }
    public bool Desc { get; }
    public DateRangeFilters Created { get; }
}