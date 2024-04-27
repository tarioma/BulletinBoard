using BulletinBoard.Application.Models.Users;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.SearchUsers;

public record SearchUsersQuery(UsersSearchFilters SearchFilters) : IRequest<IEnumerable<User>>;