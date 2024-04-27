using AutoFixture;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Users.GetUserById;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Tools;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Users;

public class GetUserByIdQueryHandlerTests
{
    private readonly IFixture _fixture = FixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_Successfully()
    {
        // Arrange
        var user = _fixture.Create<User>();

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(r => r.GetByIdAsync(
                It.Is<Guid>(i => i == user.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Users)
            .Returns(userRepositoryMock.Object);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var request = new GetUserByIdQuery(user.Id);
        var handler = new GetUserByIdQueryHandler(tenantFactoryMock.Object);

        // Act
        await handler.Handle(request);

        // Assert
        userRepositoryMock.VerifyAll();
        tenantFactoryMock.VerifyAll();
        tenantMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        GetUserByIdQuery request = null!;
        var handler = new GetUserByIdQueryHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}