using AutoFixture;
using BulletinBoard.Application.Bulletins.UpdateBulletin;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Bulletins.UpdateBulletin;

public class UpdateBulletinCommandTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var command = new UpdateBulletinCommand(id, text, rating, expiryUtc, imageStreamFactory, imageExtension);

        // Assert
        command.Id.Should().Be(id);
        command.Text.Should().Be(text);
        command.Rating.Should().Be(rating);
        command.ExpiryUtc.Should().Be(expiryUtc);
        command.ImageStream.Should().NotBeNull();
        command.ImageExtension.Should().Be(imageExtension);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_TextIsNullEmptyOrWhiteSpaceString_ThrowsArgumentException(string? text)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var action = () => new UpdateBulletinCommand(id, text!, rating, expiryUtc, imageStreamFactory, imageExtension);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }

    [Fact]
    public void Ctor_TextIsTooLongString_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var text = _fixture.CreateString(Bulletin.MaxTextLength + 1);
        var rating = _fixture.Create<int>();
        var expiryUtc = _fixture.Create<DateTime>();
        var imageStreamFactory = () => new MemoryStream();
        var imageExtension = _fixture.Create<string?>();

        // Act
        var action = () => new UpdateBulletinCommand(id, text, rating, expiryUtc, imageStreamFactory, imageExtension);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }
}