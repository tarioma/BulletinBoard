using AutoFixture;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.DeleteUser;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Users.DeleteUser;

public class DeleteUserCommandTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        // Act
        var command = new DeleteUserCommand(id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_IdIsEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var action = () => new DeleteUserCommand(id);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }
}