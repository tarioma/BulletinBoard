using AutoFixture;
using BulletinBoard.Application.Bulletins.DeleteBulletin;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Domain.Entities;
using BulletinBoard.Domain.Tests.Tools;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Bulletins;

public class DeleteBulletinCommandHandlerTests
{
    private readonly IFixture _fixture = FixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_Successfully()
    {
        // Arrange
        var imageStream = new MemoryStream();
        var imageExtension = _fixture.Create<string>();
        var image = _fixture.Create<string>();
        var bulletin = _fixture.Create<Bulletin>();

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.GetByIdAsync(
                It.Is<Guid>(i => i == bulletin.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletin);
        bulletinRepositoryMock
            .Setup(r => r.DeleteAsync(
                It.Is<Guid>(i => i == bulletin.Id),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Bulletins)
            .Returns(bulletinRepositoryMock.Object);
        tenantMock
            .Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var imageServiceMock = new Mock<IImageService>(MockBehavior.Strict);
        imageServiceMock
            .Setup(f => f.DeleteImageAsync(
                It.Is<string>(s => s == bulletin.Image),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new DeleteBulletinCommand(bulletin.Id);
        var handler = new DeleteBulletinCommandHandler(tenantFactoryMock.Object, imageServiceMock.Object);

        // Act
        await handler.Handle(request);

        // Assert
        bulletinRepositoryMock.VerifyAll();
        tenantFactoryMock.VerifyAll();
        tenantMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        DeleteBulletinCommand request = null!;
        var imageServiceMock = new Mock<IImageService>();
        var handler = new DeleteBulletinCommandHandler(tenantFactoryMock.Object, imageServiceMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}