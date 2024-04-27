namespace BulletinBoard.Application.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(Stream stream, string fileExtension, CancellationToken cancellationToken);
    Task DeleteImageAsync(string imageFileName, CancellationToken cancellationToken);
}