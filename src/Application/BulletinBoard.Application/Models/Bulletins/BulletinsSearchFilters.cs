using Ardalis.GuardClauses;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Models.Bulletins;

public class BulletinsSearchFilters
{
    private const string DefaultSortOption = nameof(Bulletin.CreatedUtc);

    private static readonly string[] SortOptions = [
        nameof(Bulletin.CreatedUtc),
        nameof(Bulletin.Number),
        nameof(Bulletin.Text),
        nameof(Bulletin.Rating),
        nameof(Bulletin.ExpiryUtc)];

    public BulletinsSearchFilters(
        PageFilter page,
        int? searchNumber,
        string? searchText,
        Guid? searchUserId,
        string? sortBy,
        bool desc,
        BulletinsRatingFilter rating,
        DateRangeFilters created,
        DateRangeFilters expiry)
    {
        Guard.Against.Null(page);
        Guard.Against.Null(created);

        if (searchText is not null)
        {
            Guard.Against.WhiteSpace(
                searchText,
                nameof(searchText),
                "Не может быть пустой строкой.");

            Guard.Against.StringTooLong(
                searchText,
                Bulletin.MaxTextLength,
                nameof(searchText),
                $"Максимальная длина: {nameof(Bulletin.MaxTextLength)}.");
        }

        if (sortBy is not null && !SortOptions.Any(s => s.Equals(sortBy, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new ArgumentException(
                $"Неверный параметр сортировки. Возможные варианты: {string.Join(", ", SortOptions)}.", nameof(sortBy));
        }

        Page = page;
        SearchNumber = searchNumber;
        SearchText = searchText;
        SearchUserId = searchUserId;
        SortBy = sortBy ?? DefaultSortOption;
        Desc = desc;
        Rating = rating;
        Created = created;
        Expiry = expiry;
    }

    public PageFilter Page { get; }
    public int? SearchNumber { get; }
    public string? SearchText { get; }
    public Guid? SearchUserId { get; }
    public string SortBy { get; }
    public bool Desc { get; }
    public BulletinsRatingFilter Rating { get; }
    public DateRangeFilters Created { get; }
    public DateRangeFilters Expiry  { get; }
}