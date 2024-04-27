using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using BulletinBoard.Domain.Common;

namespace BulletinBoard.Domain.Entities;

public class Bulletin : BaseEntity
{
    public const int MaxTextLength = 1000;

    internal Bulletin(
        Guid id,
        int number,
        string text,
        int rating,
        DateTime createdUtc,
        DateTime expiryUtc,
        Guid userId) : base(id, createdUtc)
    {
        Guard.Against.Default(userId);

        Number = number;
        SetText(text);
        Rating = rating;
        SetExpiryUtc(expiryUtc);
        UserId = userId;
    }

    public int Number { get; }
    public string Text { get; private set; }
    public int Rating { get; set; }
    public DateTime ExpiryUtc { get; private set; }
    public Guid UserId { get; }
    public string? Image { get; set; }

    public static Bulletin Create(string text, int rating, DateTime expiryUtc, Guid userId)
    {
        var id = Guid.NewGuid();
        const int number = 0;
        var createdUtc = DateTime.UtcNow;

        return new Bulletin(id, number, text, rating, createdUtc, expiryUtc, userId);
    }

    [MemberNotNull(nameof(Text))]
    public void SetText(string text)
    {
        Guard.Against.NullOrWhiteSpace(text);
        Guard.Against.StringTooLong(text, MaxTextLength);

        Text = text;
    }

    public void SetExpiryUtc(DateTime expiryUtc)
    {
        Guard.Against.Default(expiryUtc);

        if (expiryUtc < CreatedUtc)
        {
            throw new ArgumentException("Дата истечения должна быть после даты публикации.", nameof(expiryUtc));
        }

        ExpiryUtc = expiryUtc;
    }
}