using AutoFixture;
using BulletinBoard.Application.Bulletins.UpdateBulletin;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Bulletins.UpdateBulletin;

public class UpdateBulletinCommandHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_Successfully()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();
        var imageStream = () => new MemoryStream();
        var imageExtension = _fixture.Create<string>();
        var image = _fixture.Create<string>();

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.GetByIdAsync(
                It.Is<Guid>(i => i == bulletin.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletin);
        bulletinRepositoryMock.Setup(r => r.UpdateAsync(
                It.Is<Bulletin>(b => b.Id == bulletin.Id &&
                                     b.Text == bulletin.Text &&
                                     b.Rating == bulletin.Rating &&
                                     b.ExpiryUtc == bulletin.ExpiryUtc),
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
            .Setup(f => f.SaveImageAsync(
                It.IsAny<Stream>(),
                It.Is<string>(s => s == imageExtension),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(image);
        imageServiceMock
            .Setup(f => f.DeleteImageAsync(
                It.Is<string>(s => s == bulletin.Image),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new UpdateBulletinCommand(
            bulletin.Id,
            bulletin.Text,
            bulletin.Rating,
            bulletin.ExpiryUtc,
            imageStream,
            imageExtension);
        var handler = new UpdateBulletinCommandHandler(tenantFactoryMock.Object, imageServiceMock.Object);

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
        UpdateBulletinCommand request = null!;
        var imageServiceMock = new Mock<IImageService>();
        var handler = new UpdateBulletinCommandHandler(tenantFactoryMock.Object, imageServiceMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}