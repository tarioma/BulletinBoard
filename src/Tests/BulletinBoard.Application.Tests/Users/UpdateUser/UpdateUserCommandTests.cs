using AutoFixture;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.UpdateUser;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Users.UpdateUser;

public class UpdateUserCommandTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();

        // Act
        var command = new UpdateUserCommand(name, isAdmin);

        // Assert
        command.Name.Should().Be(name);
        command.IsAdmin.Should().Be(isAdmin);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_NameIsNullEmptyOrWhiteSpaceString_ThrowsArgumentException(string? name)
    {
        // Arrange
        var isAdmin = _fixture.Create<bool>();

        // Act
        var action = () => new UpdateUserCommand(name!, isAdmin);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Fact]
    public void Ctor_NameIsTooLongString_ThrowsArgumentException()
    {
        // Arrange
        var name = _fixture.CreateString(User.MaxNameLength + 1);
        var isAdmin = _fixture.Create<bool>();

        // Act
        var action = () => new UpdateUserCommand(name, isAdmin);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }
}