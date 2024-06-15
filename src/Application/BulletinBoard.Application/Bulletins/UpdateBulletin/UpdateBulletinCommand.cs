using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.UpdateBulletin;

public sealed record UpdateBulletinCommand : IRequest, IDisposable
{
    private readonly Lazy<Stream?> _lazyImageStream;

    public UpdateBulletinCommand(
        Guid id,
        string text,
        int rating,
        DateTime expiryUtc,
        Func<Stream?> imageStreamFactory,
        string? imageExtension)
    {
        Guard.Against.Default(
            id,
            nameof(id),
            "Не может иметь значение по умолчанию.");

        Guard.Against.NullOrWhiteSpace(
            text,
            nameof(text),
            "Параметр является обязательным.");

        Guard.Against.StringTooLong(
            text,
            Bulletin.MaxTextLength,
            nameof(text),
            $"Максимальная длина: {Bulletin.MaxTextLength}.");

        Guard.Against.Default(
            expiryUtc,
            nameof(expiryUtc),
            "Не может иметь значение по умолчанию.");

        Id = id;
        Text = text;
        Rating = rating;
        ExpiryUtc = expiryUtc;
        _lazyImageStream = new Lazy<Stream?>(imageStreamFactory);
        ImageExtension = imageExtension;
    }

    public Guid Id { get; init; }
    public string Text { get; }
    public int Rating { get; }
    public DateTime ExpiryUtc { get; }
    public Stream? ImageStream => _lazyImageStream.Value;
    public string? ImageExtension { get; }

    public void Dispose()
    {
        if (_lazyImageStream.IsValueCreated)
        {
            _lazyImageStream.Value?.Dispose();
        }
    }
}