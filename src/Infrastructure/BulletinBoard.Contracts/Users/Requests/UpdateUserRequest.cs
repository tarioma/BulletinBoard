namespace BulletinBoard.Contracts.Users.Requests;

public record UpdateUserRequest(Guid? Id, string Name, bool IsAdmin);