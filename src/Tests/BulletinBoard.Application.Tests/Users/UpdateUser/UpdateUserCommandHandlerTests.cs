using AutoFixture;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Application.Users.UpdateUser;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Users.UpdateUser;

public class UpdateUserCommandHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

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
        userRepositoryMock.Setup(r => r.UpdateAsync(
                It.Is<User>(u => u.Id == user.Id &&
                                 u.Name == user.Name &&
                                 u.IsAdmin == user.IsAdmin),
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

        var request = new UpdateUserCommand(user.Name, user.IsAdmin)
        {
            Id = user.Id
        };
        var handler = new UpdateUserCommandHandler(tenantFactoryMock.Object);

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
        UpdateUserCommand request = null!;
        var handler = new UpdateUserCommandHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}