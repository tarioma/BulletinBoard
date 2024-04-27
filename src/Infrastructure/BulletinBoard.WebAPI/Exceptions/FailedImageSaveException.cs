namespace BulletinBoard.WebAPI.Exceptions;

public class FailedImageSaveException : Exception
{
    public FailedImageSaveException() : base("Не удалось сохранить изображение.") { }
}