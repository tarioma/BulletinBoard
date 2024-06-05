namespace BulletinBoard.Contracts.Users.Requests;

public record UpdateUserRequest(string Name, bool IsAdmin);