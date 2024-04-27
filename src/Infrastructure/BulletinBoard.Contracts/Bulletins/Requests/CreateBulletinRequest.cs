using Microsoft.AspNetCore.Http;

namespace BulletinBoard.Contracts.Bulletins.Requests;

public record CreateBulletinRequest(string Text, int Rating, DateTimeOffset Expiry, Guid UserId, IFormFile? Image);