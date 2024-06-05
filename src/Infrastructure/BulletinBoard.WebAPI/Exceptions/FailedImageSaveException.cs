namespace BulletinBoard.WebAPI.Exceptions;

public class FailedImageSaveException : Exception
{
    public FailedImageSaveException(string text) : base($"Не удалось сохранить изображение. {text}")
    {
    }
}