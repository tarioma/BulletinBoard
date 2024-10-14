using BulletinBoard.Application.Abstraction.Models.Commands;

namespace BulletinBoard.WebAPI.Models.Requests;

public class CreateUserRequest : ICreateUserCommand
{
    public string Name { get; init; } = null!;
    public bool IsAdmin { get; init; }
}