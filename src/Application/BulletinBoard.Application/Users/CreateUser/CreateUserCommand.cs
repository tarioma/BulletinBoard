using Ardalis.GuardClauses;
using BulletinBoard.Domain.Entities;
using MediatR;

namespace BulletinBoard.Application.Users.CreateUser;

public class CreateUserCommand : IRequest<Guid>
{
    public CreateUserCommand(string name, bool isAdmin)
    {
        Guard.Against.NullOrWhiteSpace(
            name,
            nameof(name),
            "Параметр является обязательным.");

        Guard.Against.StringTooLong(
            name,
            User.MaxNameLength,
            nameof(name),
            $"Максимальная длина: {User.MaxNameLength}.");

        Name = name;
        IsAdmin = isAdmin;
    }

    public string Name { get; }
    public bool IsAdmin { get; }
}