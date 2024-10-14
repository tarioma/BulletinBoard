﻿using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Application.Abstraction.Repositories;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users;

public class SearchUsersQueryHandler : IRequestHandler<ISearchUsersQuery, User[]>
{
    private readonly IUserRepository _users;

    public SearchUsersQueryHandler(IUserRepository users)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
    }

    public async Task<User[]> Handle(ISearchUsersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _users.SearchAsync(
            request.Page,
            request.Text,
            request.IsAdmin,
            request.SortBy,
            request.Desc,
            request.Created,
            cancellationToken);
    }
}