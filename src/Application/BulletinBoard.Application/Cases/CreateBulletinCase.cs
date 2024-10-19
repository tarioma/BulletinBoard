using AutoMapper;
using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Cases;

public class CreateBulletinCase : IRequestHandler<ICreateBulletinCommand, Guid>
{
    private readonly IBulletinRepository _bulletins;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBulletinCase(IBulletinRepository bulletins, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bulletins = bulletins ?? throw new ArgumentNullException(nameof(bulletins));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Guid> Handle(ICreateBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bulletin = _mapper.Map<Bulletin>(request);

        await _bulletins.CreateAsync(bulletin, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return bulletin.Id;
    }
}