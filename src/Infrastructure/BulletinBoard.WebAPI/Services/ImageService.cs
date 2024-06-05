using BulletinBoard.Application.Services;
using BulletinBoard.WebAPI.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BulletinBoard.WebAPI.Services;

public class ImageService : IImageService
{
    private const long MaxFileSizeMegabytes = 10;
    private const long MaxFileSizeBytes = MaxFileSizeMegabytes * 1024 * 1024;

    private static readonly string[] _supportedImageFileExtensions = [".png", ".jpg", ".jpeg"];
    private static readonly string _previewImagesDirectoryPath = Path.Combine("wwwroot", "images", "preview");
    private static readonly string _fullImagesDirectoryPath = Path.Combine("wwwroot", "images", "full");

    public async Task<string> SaveImageAsync(Stream stream, string fileExtension, CancellationToken cancellationToken)
    {
        if (!_supportedImageFileExtensions.Contains(fileExtension))
        {
            throw new InvalidFileFormatException();
        }

        if (stream.Length > MaxFileSizeBytes)
        {
            throw new ImageTooLargeException($"Максимальный размер изображения: {MaxFileSizeMegabytes} мегабайт.");
        }

        CreateImageDirectories();

        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var previewImagePath = Path.Combine(_previewImagesDirectoryPath, fileName);
        var fullImagePath = Path.Combine(_fullImagesDirectoryPath, fileName);

        try
        {
            // Сохранение в полном размере
            await using (var fullImageFile = new FileStream(fullImagePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fullImageFile, cancellationToken);
            }

            // Сохранение в уменьшенном размере
            await using (var fullImageFile = new FileStream(fullImagePath, FileMode.Open, FileAccess.Read))
            {
                await SaveResizedImageAsync(fullImageFile, fileExtension, previewImagePath, cancellationToken);
            }
        }
        catch (Exception e)
        {
            throw new FailedImageSaveException(e.Message);
        }

        return fileName;
    }

    public Task DeleteImageAsync(string fileName, CancellationToken cancellationToken)
    {
        var previewImagePath = Path.Combine(_previewImagesDirectoryPath, fileName);
        var fullImagePath = Path.Combine(_fullImagesDirectoryPath, fileName);

        DeleteImageFiles(previewImagePath, fullImagePath);

        return Task.CompletedTask;
    }

    private static async Task SaveResizedImageAsync(
        Stream stream,
        string fileExtension,
        string previewImagePath,
        CancellationToken cancellationToken)
    {
        using var image = await Image.LoadAsync<Rgba32>(stream, cancellationToken);

        var newWidth = image.Width / 4;
        var newHeight = image.Height / 4;
        image.Mutate(x => x.Resize(newWidth, newHeight));

        var encoder = GetEncoder(fileExtension);

        await using var outputStream = File.Create(previewImagePath);
        await image.SaveAsync(outputStream, encoder, cancellationToken);
    }

    private static IImageEncoder GetEncoder(string fileExtension) => fileExtension switch
    {
        ".png" => new PngEncoder(),
        ".jpg" or ".jpeg" => new JpegEncoder(),
        _ => throw new InvalidFileFormatException()
    };

    private static void CreateImageDirectories()
    {
        Directory.CreateDirectory(_previewImagesDirectoryPath);
        Directory.CreateDirectory(_fullImagesDirectoryPath);
    }

    private static void DeleteImageFiles(params string[] imagePaths)
    {
        foreach (var p in imagePaths)
        {
            File.Delete(p);
        }
    }
}