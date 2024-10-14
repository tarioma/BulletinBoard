namespace BulletinBoard.Domain.Entities;

public class Bulletin
{
    public Guid Id { get; init; }
    public int Number { get; set; }
    public string Text { get; set; } = null!;
    public DateTime ExpiryUtc { get; set; }
    public string? Image { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedUtc { get; init; }
}