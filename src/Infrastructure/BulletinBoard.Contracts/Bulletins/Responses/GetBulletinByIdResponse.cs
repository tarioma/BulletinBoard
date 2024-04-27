namespace BulletinBoard.Contracts.Bulletins.Responses;

public record GetBulletinByIdResponse(
    Guid Id,
    int Number,
    string Text,
    int Rating,
    string CreatedUtc,
    string ExpiryUtc,
    Guid UserId,
    string? ImagePreview,
    string? ImageFull);