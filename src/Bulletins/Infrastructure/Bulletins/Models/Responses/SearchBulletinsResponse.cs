namespace Bulletins.Models.Responses;

public class SearchBulletinsResponse
{
    public GetBulletinByIdResponse[] Bulletins { get; init; } = [];
}