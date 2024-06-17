using AutoFixture;
using BulletinBoard.Application.Bulletins.SearchBulletins;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Tools;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Bulletins;

public class SearchBulletinsQueryHandlerTests
{
    private readonly IFixture _fixture = FixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidQuery_ReturnsIEnumerableWithSingleUser()
    {
        // Arrange
        var searchFilters = _fixture.Create<BulletinsSearchFilters>();
        var bulletin = _fixture.Create<Bulletin>();

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.SearchAsync(
                It.Is<BulletinsSearchFilters>(f => f.Page == searchFilters.Page &&
                                                   f.Count == searchFilters.Count &&
                                                   f.SearchNumber == searchFilters.SearchNumber &&
                                                   f.SearchUserId == searchFilters.SearchUserId &&
                                                   f.SortBy == searchFilters.SortBy &&
                                                   f.Desc == searchFilters.Desc &&
                                                   f.RatingFrom == searchFilters.RatingFrom &&
                                                   f.RatingTo == searchFilters.RatingTo &&
                                                   f.CreatedFromUtc == searchFilters.CreatedFromUtc &&
                                                   f.CreatedToUtc == searchFilters.CreatedToUtc &&
                                                   f.ExpiryFromUtc == searchFilters.ExpiryFromUtc &&
                                                   f.ExpiryToUtc == searchFilters.ExpiryToUtc),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { bulletin });

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Bulletins)
            .Returns(bulletinRepositoryMock.Object);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var query = new SearchBulletinsQuery(searchFilters);
        var handler = new SearchBulletinsQueryHandler(tenantFactoryMock.Object);

        // Act
        var bulletins = await handler.Handle(query);

        // Assert
        bulletinRepositoryMock.VerifyAll();
        tenantMock.VerifyAll();
        tenantFactoryMock.VerifyAll();

        bulletins.Should().ContainSingle(b => b == bulletin);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        SearchBulletinsQuery request = null!;
        var handler = new SearchBulletinsQueryHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}