using AutoFixture;
using BulletinBoard.Application.Bulletins.CreateBulletin;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Bulletins.CreateBulletin;

public class CreateBulletinCommandTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var userId = _fixture.Create<Guid>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var command = new CreateBulletinCommand(text, rating, expiryUtc, userId, imageStreamFactory, imageExtension);

        // Assert
        command.Text.Should().Be(text);
        command.Rating.Should().Be(rating);
        command.ExpiryUtc.Should().Be(expiryUtc);
        command.UserId.Should().Be(userId);
        command.ImageStream.Should().NotBeNull();
        command.ImageExtension.Should().Be(imageExtension);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_NameIsNullEmptyOrWhiteSpaceString_ThrowsArgumentException(string? text)
    {
        // Arrange
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var userId = _fixture.Create<Guid>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var action = () => new CreateBulletinCommand(
            text!, rating, expiryUtc, userId, imageStreamFactory, imageExtension);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }

    [Fact]
    public void Ctor_NameIsTooLongString_ThrowsArgumentException()
    {
        // Arrange
        var text = _fixture.CreateString(Bulletin.MaxTextLength + 1);
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var userId = _fixture.Create<Guid>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var action = () => new CreateBulletinCommand(
            text, rating, expiryUtc, userId, imageStreamFactory, imageExtension);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }
}