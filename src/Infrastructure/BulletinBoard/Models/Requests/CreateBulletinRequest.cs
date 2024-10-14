using BulletinBoard.Application.Abstraction.Models.Commands;

namespace BulletinBoard.WebAPI.Models.Requests;

public class CreateBulletinRequest : ICreateBulletinCommand
{
    public string Text { get; init; } = null!;
    public DateTime ExpiryUtc { get; init; }
    public string? Image { get; init; }
    public Guid UserId { get; init; }
}