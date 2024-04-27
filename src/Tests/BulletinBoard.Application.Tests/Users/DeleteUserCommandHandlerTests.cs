using AutoFixture;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Users.DeleteUser;
using BulletinBoard.Domain.Tests.Tools;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Users;

public class DeleteUserCommandHandlerTests
{
    private readonly IFixture _fixture = FixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_Successfully()
    {
        // Arrange
        var id = _fixture.Create<Guid>();

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(r => r.DeleteAsync(
                It.Is<Guid>(i => i == id),
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

        var request = new DeleteUserCommand(id);
        var handler = new DeleteUserCommandHandler(tenantFactoryMock.Object);

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
        DeleteUserCommand request = null!;
        var handler = new DeleteUserCommandHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}