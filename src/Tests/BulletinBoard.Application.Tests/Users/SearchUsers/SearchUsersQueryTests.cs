using AutoFixture;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.SearchUsers;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Users.SearchUsers;

public class SearchUsersQueryTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var searchFilters = _fixture.Create<UsersSearchFilters>();

        // Act
        var command = new SearchUsersQuery(searchFilters);

        // Assert
        command.SearchFilters.Should().Be(searchFilters);
    }

    [Fact]
    public void Ctor_FiltersIsNull_ThrowsArgumentException()
    {
        // Arrange
        UsersSearchFilters searchFilters = null!;

        // Act
        var action = () => new SearchUsersQuery(searchFilters);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(nameof(searchFilters));
    }
}