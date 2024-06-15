namespace BulletinBoard.Application.Exceptions;

public class LimitReachedException(string message) : Exception(message);