using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using MediatR;

namespace BulletinBoard.Application.Bulletins.UpdateBulletin;

public class UpdateBulletinCommandHandler : BaseHandler, IRequestHandler<UpdateBulletinCommand>
{
    private readonly IImageService _imageService;

    public UpdateBulletinCommandHandler(ITenantFactory tenantFactory, IImageService imageService) : base(tenantFactory)
    {
        Guard.Against.Null(imageService);

        _imageService = imageService;
    }

    public async Task Handle(UpdateBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        var bulletin = await tenant.Bulletins.GetByIdAsync(request.Id, cancellationToken);

        if (request.ImageStream is not null && request.ImageExtension is not null)
        {
            var newImage = request.ImageStream is not null && request.ImageExtension is not null
                ? await _imageService.SaveImageAsync(
                    request.ImageStream,
                    request.ImageExtension,
                    cancellationToken)
                : null;

            bulletin.Image = newImage;

            if (bulletin.Image is not null)
            {
                await _imageService.DeleteImageAsync(bulletin.Image, cancellationToken);
            }
        }

        bulletin.SetText(request.Text);
        bulletin.Rating = request.Rating;
        bulletin.SetExpiryUtc(request.ExpiryUtc);

        await tenant.Bulletins.UpdateAsync(bulletin, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
    }
}