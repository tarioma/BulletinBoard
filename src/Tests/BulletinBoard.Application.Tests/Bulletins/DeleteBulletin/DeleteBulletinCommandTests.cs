using AutoFixture;
using BulletinBoard.Application.Bulletins.DeleteBulletin;
using BulletinBoard.Application.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Bulletins.DeleteBulletin;

public class DeleteBulletinCommandTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        // Act
        var command = new DeleteBulletinCommand(id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_IdIsEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var action = () => new DeleteBulletinCommand(id);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }
}