namespace BulletinBoard.Contracts.Users.Responses;

public record SearchUsersResponse(IEnumerable<GetUserByIdResponse> Users);