using Ardalis.GuardClauses;
using BulletinBoard.Application.Exceptions;
using BulletinBoard.Application.Options;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace BulletinBoard.Application.Bulletins.CreateBulletin;

public class CreateBulletinCommandHandler(
    IBulletinRepository bulletins,
    IUnitOfWork unitOfWork,
    IImageService imageService,
    IOptions<BulletinsConfigurationOptions> configurationOptions)
    : IRequestHandler<CreateBulletinCommand, Guid>
{
    private readonly BulletinsConfigurationOptions _bulletinsConfigurationOptions = configurationOptions.Value;
    
    public async Task<Guid> Handle(CreateBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var userBulletinsCount = await bulletins.GetUserBulletinsCountAsync(request.UserId, cancellationToken);

        if (userBulletinsCount >= _bulletinsConfigurationOptions.MaxBulletinsCountPerUser)
        {
            throw new LimitReachedException(
                $"Достигнут лимит на количество объявлений ({_bulletinsConfigurationOptions.MaxBulletinsCountPerUser}).");
        }

        var bulletin = Bulletin.Create(request.Text, request.Rating, request.ExpiryUtc, request.UserId);
        bulletin.Image = request.ImageStream is not null && request.ImageExtension is not null
            ? await imageService.SaveImageAsync(
                request.ImageStream,
                request.ImageExtension,
                cancellationToken)
            : null;

        await bulletins.CreateAsync(bulletin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return bulletin.Id;
    }
}