namespace BulletinBoard.WebAPI.Models.Responses;

public class SearchUsersResponse
{
    public GetUserByIdResponse[] Users { get; init; } = null!;
}