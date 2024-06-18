using AutoFixture;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Models.Users;

public class UsersSearchFiltersTests
{
    private readonly IFixture _fixture = DomainFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchName = _fixture.Create<string?>();
        var searchIsAdmin = _fixture.Create<bool?>();
        const string sortBy = nameof(User.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var created = _fixture.Create<DateRangeFilters>();

        // Act
        var user = new UsersSearchFilters(page, searchName, searchIsAdmin, sortBy, desc, created);

        // Assert
        user.Page.Should().Be(page);
        user.SearchName.Should().Be(searchName);
        user.SearchIsAdmin.Should().Be(searchIsAdmin);
        user.SortBy.Should().Be(sortBy);
        user.Desc.Should().Be(desc);
        user.Created.Should().Be(created);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_SearchNameIsEmptyOrWhiteSpace_ThrowsArgumentException(string searchName)
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchIsAdmin = _fixture.Create<bool?>();
        const string sortBy = nameof(User.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var created = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new UsersSearchFilters(page, searchName, searchIsAdmin, sortBy, desc, created);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(searchName));
    }

    [Fact]
    public void Ctor_SearchNameIsTooLongString_ThrowsArgumentException()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchName = _fixture.CreateString(User.MaxNameLength + 1);
        var searchIsAdmin = _fixture.Create<bool?>();
        const string sortBy = nameof(User.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var created = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new UsersSearchFilters(page, searchName, searchIsAdmin, sortBy, desc, created);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(searchName));
    }

    [Fact]
    public void Ctor_SortByIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchName = _fixture.Create<string?>();
        var searchIsAdmin = _fixture.Create<bool?>();
        var sortBy = _fixture.Create<string>();
        var desc = _fixture.Create<bool>();
        var created = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new UsersSearchFilters(page, searchName, searchIsAdmin, sortBy, desc, created);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(sortBy));
    }
}