namespace BulletinBoard.Domain.Entities;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public DateTime CreatedUtc { get; init; }
}