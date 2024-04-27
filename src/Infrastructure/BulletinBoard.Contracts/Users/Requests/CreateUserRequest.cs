namespace BulletinBoard.Contracts.Users.Requests;

public record CreateUserRequest(string Name, bool IsAdmin);