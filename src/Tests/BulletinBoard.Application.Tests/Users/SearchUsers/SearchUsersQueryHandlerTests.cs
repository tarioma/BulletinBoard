using AutoFixture;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.SearchUsers;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Users.SearchUsers;

public class SearchUsersQueryHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidQuery_ReturnsIEnumerableWithSingleUser()
    {
        // Arrange
        var searchFilters = _fixture.Create<UsersSearchFilters>();
        var user = _fixture.Create<User>();

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(r => r.SearchAsync(
                It.Is<UsersSearchFilters>(f => f.Page == searchFilters.Page &&
                                               f.SearchName == searchFilters.SearchName &&
                                               f.SearchIsAdmin == searchFilters.SearchIsAdmin &&
                                               f.SortBy == searchFilters.SortBy &&
                                               f.Desc == searchFilters.Desc &&
                                               f.Created == searchFilters.Created),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { user });

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Users)
            .Returns(userRepositoryMock.Object);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var query = new SearchUsersQuery(searchFilters);
        var handler = new SearchUsersQueryHandler(tenantFactoryMock.Object);

        // Act
        var users = await handler.Handle(query);

        // Assert
        userRepositoryMock.VerifyAll();
        tenantMock.VerifyAll();
        tenantFactoryMock.VerifyAll();

        users.Should().ContainSingle(u => u == user);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        SearchUsersQuery request = null!;
        var handler = new SearchUsersQueryHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}