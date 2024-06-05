namespace BulletinBoard.Contracts.Extensions;

public static class IntExtensions
{
    public static int ClampMin(this int value, int min) => value < min ? min : value;
}