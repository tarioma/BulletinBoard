namespace Users.Models.Responses;

public class SearchUsersResponse
{
    public GetUserByIdResponse[] Users { get; init; } = [];
}