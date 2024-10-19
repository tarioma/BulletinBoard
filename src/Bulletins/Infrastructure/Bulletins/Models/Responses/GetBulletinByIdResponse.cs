namespace Bulletins.Models.Responses;

public class GetBulletinByIdResponse
{
    public Guid Id { get; init; }
    public int Number { get; init; }
    public string Text { get; init; } = null!;
    public DateTime CreatedUtc { get; init; }
    public DateTime ExpiryUtc { get; init; }
    public string? Image { get; init; }
    public Guid UserId { get; init; }
}