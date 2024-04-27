namespace BulletinBoard.WebAPI.Exceptions;

public class InvalidFileFormatException : Exception
{
    public InvalidFileFormatException() : base("Недопустимый формат изображения.") { }
}