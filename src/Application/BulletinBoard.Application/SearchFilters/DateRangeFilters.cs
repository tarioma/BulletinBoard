namespace BulletinBoard.Application.SearchFilters;

public readonly struct DateRangeFilters(DateTime? from, DateTime? to)
{
    public DateTime? From { get; } = from;
    public DateTime? To { get; } = to;
}