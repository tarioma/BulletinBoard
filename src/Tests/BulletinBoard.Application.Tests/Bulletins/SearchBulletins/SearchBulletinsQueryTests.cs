using AutoFixture;
using BulletinBoard.Application.Bulletins.SearchBulletins;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Bulletins.SearchBulletins;

public class SearchBulletinsQueryTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var searchFilters = _fixture.Create<BulletinsSearchFilters>();

        // Act
        var command = new SearchBulletinsQuery(searchFilters);

        // Assert
        command.SearchFilters.Should().Be(searchFilters);
    }

    [Fact]
    public void Ctor_FiltersIsNull_ThrowsArgumentException()
    {
        // Arrange
        BulletinsSearchFilters searchFilters = null!;

        // Act
        var action = () => new SearchBulletinsQuery(searchFilters);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(nameof(searchFilters));
    }
}