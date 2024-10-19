using Bulletins.Application.Abstraction.Models.Commands;

namespace Bulletins.Models.Requests;

public class UpdateBulletinRequest : IUpdateBulletinCommand
{
    public Guid Id { get; internal set; }
    public string Text { get; init; } = null!;
    public DateTime ExpiryUtc { get; init; }
    public string? Image { get; init; }
}