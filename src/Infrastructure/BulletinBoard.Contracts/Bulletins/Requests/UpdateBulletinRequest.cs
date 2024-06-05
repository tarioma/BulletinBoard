using Microsoft.AspNetCore.Http;

namespace BulletinBoard.Contracts.Bulletins.Requests;

public record UpdateBulletinRequest(string Text, int Rating, DateTimeOffset Expiry, IFormFile? Image);