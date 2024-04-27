namespace BulletinBoard.WebAPI.Exceptions;

public class ImageTooLargeException : Exception
{
    public ImageTooLargeException(string message) : base(message)
    {
    }
}