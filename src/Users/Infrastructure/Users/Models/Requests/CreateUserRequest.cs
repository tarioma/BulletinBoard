using Users.Application.Abstraction.Models.Commands;

namespace Users.Models.Requests;

public class CreateUserRequest : ICreateUserCommand
{
    public string Name { get; init; } = null!;
    public bool IsAdmin { get; init; }
}