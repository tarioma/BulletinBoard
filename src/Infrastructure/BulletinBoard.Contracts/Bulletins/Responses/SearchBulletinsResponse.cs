namespace BulletinBoard.Contracts.Bulletins.Responses;

public record SearchBulletinsResponse(IEnumerable<GetBulletinByIdResponse> Bulletins);