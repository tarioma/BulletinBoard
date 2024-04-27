using AutoFixture;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Tools;
using FluentAssertions;

namespace BulletinBoard.Domain.Tests.Entities;

public class BulletinTests
{
    private readonly IFixture _fixture = FixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var number = _fixture.Create<int>();
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var createdUtc = _fixture.Create<DateTime>();
        var expiryUtc = createdUtc.AddDays(1);
        var userId = _fixture.Create<Guid>();

        // Act
        var bulletin = new Bulletin(id, number, text, rating, createdUtc, expiryUtc, userId);

        // Assert
        bulletin.Id.Should().Be(id);
        bulletin.Number.Should().Be(number);
        bulletin.Text.Should().Be(text);
        bulletin.Rating.Should().Be(rating);
        bulletin.CreatedUtc.Should().Be(createdUtc);
        bulletin.ExpiryUtc.Should().Be(expiryUtc);
        bulletin.UserId.Should().Be(userId);
    }

    [Fact]
    public void Ctor_UserIdIsEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var number = _fixture.Create<int>();
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var createdUtc = _fixture.Create<DateTime>();
        var expiryUtc = createdUtc.AddDays(1);
        var userId = Guid.Empty;

        // Act
        var action = () => new Bulletin(id, number, text, rating, createdUtc, expiryUtc, userId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(userId));
    }

    [Fact]
    public void Create_ValidParams_SuccessfulCreateAndReturn()
    {
        // Arrange
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var expiryUtc = DateTime.UtcNow.AddDays(1);
        var userId = _fixture.Create<Guid>();

        // Act
        var bulletin = Bulletin.Create(text, rating, expiryUtc, userId);

        // Assert
        bulletin.Id.Should().NotBeEmpty();
        bulletin.Number.Should().Be(0);
        bulletin.Text.Should().Be(text);
        bulletin.Rating.Should().Be(rating);
        bulletin.CreatedUtc.Should().NotBe(default);
        bulletin.ExpiryUtc.Should().Be(expiryUtc);
        bulletin.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public void SetText_ValidText_TextSuccessfullyChanged()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var text = _fixture.Create<string>();

        // Act
        bulletin.SetText(text);

        // Assert
        bulletin.Text.Should().Be(text);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetText_TextIsNullEmptyOrWhiteSpace_ThrowsArgumentException(string? text)
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();

        // Act
        var action = () => bulletin.SetText(text!);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }

    [Fact]
    public void SetText_TextIsTooLong_ThrowsArgumentException()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var text = _fixture.CreateString(Bulletin.MaxTextLength + 1);

        // Act
        var action = () => bulletin.SetText(text);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(text));
    }

    [Fact]
    public void SetExpiryUtc_ValidExpiryUtc_ExpiryUtcSuccessfullyChanged()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var expiryUtc = bulletin.CreatedUtc.AddDays(1);

        // Act
        bulletin.SetExpiryUtc(expiryUtc);

        // Assert
        bulletin.ExpiryUtc.Should().Be(expiryUtc);
    }

    [Fact]
    public void SetExpiryUtc_ExpiryUtcIsDefault_ThrowsArgumentException()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var expiryUtc = default(DateTime);

        // Act
        var action = () => bulletin.SetExpiryUtc(expiryUtc);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(expiryUtc));
    }

    [Fact]
    public void SetExpiryUtc_ExpiryUtcIsEarlierThenCreatedUtc_ThrowsArgumentException()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var expiryUtc = bulletin.CreatedUtc.AddDays(-1);

        // Act
        var action = () => bulletin.SetExpiryUtc(expiryUtc);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(expiryUtc));
    }
}