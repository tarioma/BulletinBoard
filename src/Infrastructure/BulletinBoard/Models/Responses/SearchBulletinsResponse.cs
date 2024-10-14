namespace BulletinBoard.WebAPI.Models.Responses;

public class SearchBulletinsResponse
{
    public GetBulletinByIdResponse[] Bulletins { get; init; } = null!;
}