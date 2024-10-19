using Bulletins.Application.Abstraction.Models.Commands;

namespace Bulletins.Models.Requests;

public class CreateBulletinRequest : ICreateBulletinCommand
{
    public string Text { get; init; } = null!;
    public DateTime ExpiryUtc { get; init; }
    public string? Image { get; init; }
    public Guid UserId { get; init; }
}