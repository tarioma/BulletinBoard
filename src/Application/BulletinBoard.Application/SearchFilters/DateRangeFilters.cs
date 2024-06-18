namespace BulletinBoard.Application.SearchFilters;

public readonly struct DateRangeFilters(DateTime? from, DateTime? to)
{
    public DateTime? From { get; } = from;
    public DateTime? To { get; } = to;

    public override bool Equals(object? obj) => obj is DateRangeFilters filter && Equals(filter);

    public override int GetHashCode() => HashCode.Combine(From, To);

    public static bool operator ==(DateRangeFilters left, DateRangeFilters right) => left.Equals(right);

    public static bool operator !=(DateRangeFilters left, DateRangeFilters right) => !(left == right);

    private bool Equals(DateRangeFilters other) => From == other.From && To == other.To;
}