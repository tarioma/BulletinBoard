using BulletinBoard.Application.Abstraction.Models.Commands;

namespace BulletinBoard.WebAPI.Models.Requests;

public class DeleteBulletinRequest : IDeleteBulletinCommand
{
    public Guid Id { get; init; }
}