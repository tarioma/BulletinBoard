using BulletinBoard.Application.Abstraction.Models.Commands;

namespace BulletinBoard.WebAPI.Models.Requests;

public class UpdateBulletinRequest : IUpdateBulletinCommand
{
    // TODO
    public Guid Id { get; internal set; }
    public string Text { get; init; } = null!;
    public DateTime ExpiryUtc { get; init; }
    public string? Image { get; init; }
}