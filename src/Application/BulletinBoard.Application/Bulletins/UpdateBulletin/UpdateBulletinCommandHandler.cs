using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Application.Specifications;
using MediatR;

namespace BulletinBoard.Application.Bulletins.UpdateBulletin;

public class UpdateBulletinCommandHandler(
    IBulletinRepository bulletins,
    IUnitOfWork unitOfWork,
    IImageService imageService) : IRequestHandler<UpdateBulletinCommand>
{
    public async Task Handle(UpdateBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var bulletin = await bulletins.GetByIdAsync(
            new BulletinByIdSpecification(request.Id),
            cancellationToken);

        if (request.ImageStream is not null && request.ImageExtension is not null)
        {
            var newImage = request.ImageStream is not null && request.ImageExtension is not null
                ? await imageService.SaveImageAsync(
                    request.ImageStream,
                    request.ImageExtension,
                    cancellationToken)
                : null;

            bulletin.Image = newImage;

            if (bulletin.Image is not null)
            {
                await imageService.DeleteImageAsync(bulletin.Image, cancellationToken);
            }
        }

        bulletin.SetText(request.Text);
        bulletin.Rating = request.Rating;
        bulletin.SetExpiryUtc(request.ExpiryUtc);

        await bulletins.UpdateAsync(bulletin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}