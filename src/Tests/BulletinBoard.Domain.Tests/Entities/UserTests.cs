using AutoFixture;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Domain.Tests.Entities;

public class UserTests
{
    private readonly IFixture _fixture = DomainFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();
        var createdUtc = _fixture.Create<DateTime>();

        // Act
        var user = new User(id, name, isAdmin, createdUtc);

        // Assert
        user.Id.Should().Be(id);
        user.Name.Should().Be(name);
        user.IsAdmin.Should().Be(isAdmin);
        user.CreatedUtc.Should().Be(createdUtc);
    }

    [Fact]
    public void Ctor_IdIsEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();
        var createdUtc = _fixture.Create<DateTime>();

        // Act
        var action = () => new User(id, name, isAdmin, createdUtc);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }

    [Fact]
    public void Ctor_CreatedUtcIsDefault_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();
        var createdUtc = default(DateTime);

        // Act
        var action = () => new User(id, name, isAdmin, createdUtc);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(createdUtc));
    }

    [Fact]
    public void Create_ValidParams_SuccessfulCreateAndReturn()
    {
        // Assert
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();

        // Act
        var user = User.Create(name, isAdmin);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Name.Should().Be(name);
        user.IsAdmin.Should().Be(isAdmin);
        user.CreatedUtc.Should().NotBe(default);
    }

    [Fact]
    public void SetName_ValidName_NameSuccessfullyChanged()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var name = _fixture.Create<string>();

        // Act
        user.SetName(name);

        // Assert
        user.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetName_NullEmptyOrWhiteSpaceString_ThrowsArgumentException(string? name)
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        var action = () => user.SetName(name!);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Fact]
    public void SetName_TooLongString_ThrowsArgumentException()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var name = _fixture.CreateString(User.MaxNameLength + 1);

        // Act
        var action = () => user.SetName(name);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }
}