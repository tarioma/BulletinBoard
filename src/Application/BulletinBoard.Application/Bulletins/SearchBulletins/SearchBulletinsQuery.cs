using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Bulletins.SearchBulletins;

public record SearchBulletinsQuery(BulletinsSearchFilters SearchFilters) : IRequest<IEnumerable<Bulletin>>;