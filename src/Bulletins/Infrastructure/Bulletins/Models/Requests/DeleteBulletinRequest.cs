using Bulletins.Application.Abstraction.Models.Commands;

namespace Bulletins.Models.Requests;

public class DeleteBulletinRequest : IDeleteBulletinCommand
{
    public Guid Id { get; init; }
}