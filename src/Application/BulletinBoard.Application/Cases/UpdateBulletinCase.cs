using AutoMapper;
using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Repositories;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class UpdateBulletinCase : IRequestHandler<IUpdateBulletinCommand>
{
    private readonly IBulletinRepository _bulletins;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBulletinCase(IBulletinRepository bulletins, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(IUpdateBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bulletin = await _bulletins.GetByIdAsync(request.Id, cancellationToken);
        _mapper.Map(request, bulletin);

        await _bulletins.UpdateAsync(bulletin, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}