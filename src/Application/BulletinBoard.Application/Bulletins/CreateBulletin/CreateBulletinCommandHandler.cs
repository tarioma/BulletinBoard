using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Exceptions;
using BulletinBoard.Application.Options;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace BulletinBoard.Application.Bulletins.CreateBulletin;

public class CreateBulletinCommandHandler : BaseHandler, IRequestHandler<CreateBulletinCommand, Guid>
{
    private readonly IImageService _imageService;
    private readonly BulletinsConfigurationOptions _configurationOptions;

    public CreateBulletinCommandHandler(
        ITenantFactory tenantFactory,
        IImageService imageService,
        IOptions<BulletinsConfigurationOptions> configurationOptions)
        : base(tenantFactory)
    {
        Guard.Against.Null(imageService);

        _imageService = imageService;
        _configurationOptions = configurationOptions.Value;
    }

    public async Task<Guid> Handle(CreateBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();
        var userBulletinsCount = await tenant.Bulletins.GetUserBulletinsCountAsync(request.UserId, cancellationToken);

        if (userBulletinsCount >= _configurationOptions.MaxBulletinsCountPerUser)
        {
            throw new LimitReachedException(
                $"Достигнут лимит на количество объявлений ({_configurationOptions.MaxBulletinsCountPerUser}).");
        }

        var bulletin = Bulletin.Create(request.Text, request.Rating, request.ExpiryUtc, request.UserId);
        bulletin.Image = request.ImageStream is not null && request.ImageExtension is not null
            ? await _imageService.SaveImageAsync(
                request.ImageStream,
                request.ImageExtension,
                cancellationToken)
            : null;

        await tenant.Bulletins.CreateAsync(bulletin, cancellationToken);
        await tenant.CommitAsync(cancellationToken);

        return bulletin.Id;
    }
}