using Ardalis.GuardClauses;

namespace BulletinBoard.Application.SearchFilters;

public class PageFilter
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
}