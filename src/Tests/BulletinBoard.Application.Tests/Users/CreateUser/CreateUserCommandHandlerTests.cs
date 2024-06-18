using AutoFixture;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.CreateUser;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Users.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_ReturnsUserId()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var isAdmin = _fixture.Create<bool>();

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(r => r.CreateAsync(
                It.Is<User>(u => u.Name == name &&
                                 u.IsAdmin == isAdmin),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Users)
            .Returns(userRepositoryMock.Object);
        tenantMock
            .Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var request = new CreateUserCommand(name, isAdmin);
        var handler = new CreateUserCommandHandler(tenantFactoryMock.Object);

        // Act
        var userId = await handler.Handle(request);

        // Assert
        userRepositoryMock.VerifyAll();
        tenantFactoryMock.VerifyAll();
        tenantMock.VerifyAll();

        userId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        CreateUserCommand request = null!;
        var handler = new CreateUserCommandHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}