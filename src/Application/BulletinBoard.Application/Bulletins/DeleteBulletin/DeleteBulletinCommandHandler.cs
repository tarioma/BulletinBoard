using Ardalis.GuardClauses;
using BulletinBoard.Application.Common;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using MediatR;

namespace BulletinBoard.Application.Bulletins.DeleteBulletin;

public class DeleteBulletinCommandHandler : BaseHandler, IRequestHandler<DeleteBulletinCommand>
{
    private readonly IImageService _imageService;

    public DeleteBulletinCommandHandler(ITenantFactory tenantFactory, IImageService imageService) : base(tenantFactory)
    {
        Guard.Against.Null(imageService);

        _imageService = imageService;
    }

    public async Task Handle(DeleteBulletinCommand request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var tenant = TenantFactory.GetTenant();

        var bulletin = await tenant.Bulletins.GetByIdAsync(request.Id, cancellationToken);

        if (bulletin.Image is not null)
        {
            await _imageService.DeleteImageAsync(bulletin.Image, cancellationToken);
        }

        await tenant.Bulletins.DeleteAsync(request.Id, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
    }
}