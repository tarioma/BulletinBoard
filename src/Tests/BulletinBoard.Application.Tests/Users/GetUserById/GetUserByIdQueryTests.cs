using AutoFixture;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.GetUserById;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Users.GetUserById;

public class GetUserByIdQueryTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        // Act
        var command = new GetUserByIdQuery(id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_IdIsEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var action = () => new GetUserByIdQuery(id);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }
}