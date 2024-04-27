using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using BulletinBoard.Domain.Common;

namespace BulletinBoard.Domain.Entities;

public class User : BaseEntity
{
    public const int MaxNameLength = 100;

    internal User(Guid id, string name, bool isAdmin, DateTime createdUtc) : base(id, createdUtc)
    {
        SetName(name);
        IsAdmin = isAdmin;
    }

    public string Name { get; private set; }
    public bool IsAdmin { get; set; }

    public static User Create(string name, bool isAdmin)
    {
        var id = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        return new User(id, name, isAdmin, timestamp);
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.StringTooLong(name, MaxNameLength);

        Name = name;
    }
}