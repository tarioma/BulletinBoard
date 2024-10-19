using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Repositories;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class DeleteBulletinCase : IRequestHandler<IDeleteBulletinCommand>
{
    private readonly IBulletinRepository _bulletins;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBulletinCase(IBulletinRepository bulletins, IUnitOfWork unitOfWork)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(IDeleteBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _bulletins.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}