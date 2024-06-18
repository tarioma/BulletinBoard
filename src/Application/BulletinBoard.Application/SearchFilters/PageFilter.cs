using Ardalis.GuardClauses;

namespace BulletinBoard.Application.SearchFilters;

public readonly struct PageFilter
{
    public PageFilter(int count, int offset)
    {
        Guard.Against.NegativeOrZero(count);
        Guard.Against.Negative(offset);

        Count = count;
        Offset = offset;
    }

    public int Count { get; }
    public int Offset { get; }

    public override bool Equals(object? obj) => obj is PageFilter filter && Equals(filter);

    public override int GetHashCode() => HashCode.Combine(Count, Offset);

    public static bool operator ==(PageFilter left, PageFilter right) => left.Equals(right);

    public static bool operator !=(PageFilter left, PageFilter right) => !(left == right);

    private bool Equals(PageFilter other) => Count == other.Count && Offset == other.Offset;
}