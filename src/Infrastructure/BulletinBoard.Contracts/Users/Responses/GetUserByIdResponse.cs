namespace BulletinBoard.Contracts.Users.Responses;

public record GetUserByIdResponse(Guid Id, string Name, bool IsAdmin, string CreatedUtc);