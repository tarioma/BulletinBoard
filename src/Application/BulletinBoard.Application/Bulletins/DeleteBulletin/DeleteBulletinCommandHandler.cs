using Ardalis.GuardClauses;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using MediatR;

namespace BulletinBoard.Application.Bulletins.DeleteBulletin;

public class DeleteBulletinCommandHandler(
    IBulletinRepository bulletins,
    IUnitOfWork unitOfWork,
    IImageService imageService)
    : IRequestHandler<DeleteBulletinCommand>
{
    public async Task Handle(DeleteBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var bulletin = await bulletins.GetByIdAsync(request.Id, cancellationToken);

        if (bulletin.Image is not null)
        {
            await imageService.DeleteImageAsync(bulletin.Image, cancellationToken);
        }

        await bulletins.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}