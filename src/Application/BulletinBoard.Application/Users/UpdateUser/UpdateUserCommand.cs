﻿using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public UpdateUserCommand(string name, bool isAdmin)
    {
        Guard.Against.NullOrWhiteSpace(
            name,
            nameof(name),
            $"Параметр {nameof(name)} является обязательным.");

        Guard.Against.StringTooLong(
            name,
            User.MaxNameLength,
            nameof(name),
            $"Максимальная длина {nameof(name)}: {User.MaxNameLength}.");

        Name = name;
        IsAdmin = isAdmin;
    }

    public Guid Id { get; set; }
    public string Name { get; }
    public bool IsAdmin { get; }
}