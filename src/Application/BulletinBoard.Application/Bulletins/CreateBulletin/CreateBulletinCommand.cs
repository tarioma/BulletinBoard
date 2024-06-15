using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.CreateBulletin;

public sealed record CreateBulletinCommand : IRequest<Guid>, IDisposable
{
    private readonly Lazy<Stream?> _lazyImageStream;

    public CreateBulletinCommand(
        string text,
        int rating,
        DateTime expiryUtc,
        Guid userId,
        Func<Stream?> imageStreamFactory,
        string? imageExtension)
    {
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

        Guard.Against.Default(
            userId,
            nameof(userId),
            "Не может иметь значение по умолчанию.");

        Text = text;
        Rating = rating;
        ExpiryUtc = expiryUtc;
        UserId = userId;
        _lazyImageStream = new Lazy<Stream?>(imageStreamFactory);
        ImageExtension = imageExtension;
    }

    public string Text { get; }
    public int Rating { get; }
    public DateTime ExpiryUtc { get; }
    public Guid UserId { get; }
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