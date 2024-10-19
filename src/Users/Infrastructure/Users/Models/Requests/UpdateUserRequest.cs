using Users.Application.Abstraction.Models.Commands;

namespace Users.Models.Requests;

public class UpdateUserRequest : IUpdateUserCommand
{
    public Guid Id { get; internal set; }
    public string Name { get; init; } = null!;
    public bool IsAdmin { get; init; }
}