using Users.Application.Abstraction.Models.Commands;

namespace Users.Models.Requests;

public class DeleteUserRequest : IDeleteUserCommand
{
    public Guid Id { get; init; }
}