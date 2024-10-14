using BulletinBoard.Application.Abstraction.Models.Commands;

namespace BulletinBoard.WebAPI.Models.Requests;

public class DeleteUserRequest : IDeleteUserCommand
{
    public Guid Id { get; init; }
}