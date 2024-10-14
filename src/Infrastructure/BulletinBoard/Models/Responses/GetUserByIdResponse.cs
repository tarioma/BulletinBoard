namespace BulletinBoard.WebAPI.Models.Responses;

public class GetUserByIdResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public bool IsAdmin { get; init; }
    public DateTime CreatedUtc { get; init; }
}