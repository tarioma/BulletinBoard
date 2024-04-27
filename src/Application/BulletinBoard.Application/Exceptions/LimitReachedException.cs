namespace BulletinBoard.Application.Exceptions;

public class LimitReachedException : Exception
{
    public LimitReachedException(string message) : base(message) { }
}