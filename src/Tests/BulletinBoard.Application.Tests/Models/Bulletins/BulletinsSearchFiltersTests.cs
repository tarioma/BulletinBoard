using AutoFixture;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Extensions;
using FluentAssertions;

namespace BulletinBoard.Application.Tests.Models.Bulletins;

public class BulletinsSearchFiltersTests
{
    private readonly IFixture _fixture = DomainFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public void Ctor_ValidParams_SuccessfulInit()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchNumber = _fixture.Create<int?>();
        var searchText = _fixture.Create<string?>();
        var searchUserId = _fixture.Create<Guid?>();
        const string sortBy = nameof(Bulletin.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var rating = _fixture.Create<BulletinsRatingFilter>();
        var created = _fixture.Create<DateRangeFilters>();
        var expiry = _fixture.Create<DateRangeFilters>();

        // Act
        var filters = new BulletinsSearchFilters(
            page, searchNumber, searchText, searchUserId, sortBy, desc, rating, created, expiry);

        // Assert
        filters.Page.Should().Be(page);
        filters.SearchNumber.Should().Be(searchNumber);
        filters.SearchText.Should().Be(searchText);
        filters.SearchUserId.Should().Be(searchUserId);
        filters.SortBy.Should().Be(sortBy);
        filters.Desc.Should().Be(desc);
        filters.Rating.Should().Be(rating);
        filters.Created.Should().Be(created);
        filters.Expiry.Should().Be(expiry);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_SearchTextIsEmptyOrWhiteSpace_ThrowsArgumentException(string searchText)
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchNumber = _fixture.Create<int?>();
        var searchUserId = _fixture.Create<Guid?>();
        const string sortBy = nameof(Bulletin.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var rating = _fixture.Create<BulletinsRatingFilter>();
        var created = _fixture.Create<DateRangeFilters>();
        var expiry = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new BulletinsSearchFilters(
            page, searchNumber, searchText, searchUserId, sortBy, desc, rating, created, expiry);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(searchText));
    }

    [Fact]
    public void Ctor_SearchTextIsTooLongString_ThrowsArgumentException()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchNumber = _fixture.Create<int?>();
        var searchText = _fixture.CreateString(Bulletin.MaxTextLength + 1);
        var searchUserId = _fixture.Create<Guid?>();
        const string sortBy = nameof(Bulletin.CreatedUtc);
        var desc = _fixture.Create<bool>();
        var rating = _fixture.Create<BulletinsRatingFilter>();
        var created = _fixture.Create<DateRangeFilters>();
        var expiry = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new BulletinsSearchFilters(
            page, searchNumber, searchText, searchUserId, sortBy, desc, rating, created, expiry);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(searchText));
    }

    [Fact]
    public void Ctor_SortByIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var page = _fixture.Create<PageFilter>();
        var searchNumber = _fixture.Create<int?>();
        var searchText = _fixture.Create<string?>();
        var searchUserId = _fixture.Create<Guid?>();
        var sortBy = _fixture.Create<string>();
        var desc = _fixture.Create<bool>();
        var rating = _fixture.Create<BulletinsRatingFilter>();
        var created = _fixture.Create<DateRangeFilters>();
        var expiry = _fixture.Create<DateRangeFilters>();

        // Act
        var action = () => new BulletinsSearchFilters(
            page, searchNumber, searchText, searchUserId, sortBy, desc, rating, created, expiry);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(sortBy));
    }
}