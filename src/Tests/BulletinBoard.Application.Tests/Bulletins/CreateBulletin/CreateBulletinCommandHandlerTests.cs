using AutoFixture;
using BulletinBoard.Application.Bulletins.CreateBulletin;
using BulletinBoard.Application.Exceptions;
using BulletinBoard.Application.Options;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Services;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace BulletinBoard.Application.Tests.Bulletins.CreateBulletin;

public class CreateBulletinCommandHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_ReturnsUserId()
    {
        // Arrange
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var imageStream = () => new MemoryStream();
        var imageExtension = _fixture.Create<string>();
        var expiryUtc = DateTime.UtcNow.AddDays(1);
        var userId = _fixture.Create<Guid>();
        var image = _fixture.Create<string>();
        var bulletinsConfigurationOptions = new BulletinsConfigurationOptions { MaxBulletinsCountPerUser = 1 };

        var configurationOptionsMock = new Mock<IOptions<BulletinsConfigurationOptions>>(MockBehavior.Strict);
        configurationOptionsMock
            .Setup(c => c.Value)
            .Returns(bulletinsConfigurationOptions);

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.CreateAsync(
                It.Is<Bulletin>(b => b.Text == text &&
                                     b.Rating == rating &&
                                     b.ExpiryUtc == expiryUtc &&
                                     b.UserId == userId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        bulletinRepositoryMock
            .Setup(r => r.GetUserBulletinsCountAsync(
                It.Is<Guid>(i => i == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

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

        var request = new CreateBulletinCommand(text, rating, expiryUtc, userId, imageStream, imageExtension);
        var handler = new CreateBulletinCommandHandler(
            tenantFactoryMock.Object,
            imageServiceMock.Object,
            configurationOptionsMock.Object);

        // Act
        var bulletinId = await handler.Handle(request);

        // Assert
        bulletinRepositoryMock.VerifyAll();
        tenantFactoryMock.VerifyAll();
        tenantMock.VerifyAll();
        imageServiceMock.VerifyAll();

        bulletinId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_BulletinsCountLimitReached_ReturnsUserId()
    {
        // Arrange
        var text = _fixture.Create<string>();
        var rating = _fixture.Create<int>();
        var imageStream = () => new MemoryStream();
        var imageExtension = _fixture.Create<string>();
        var expiryUtc = DateTime.UtcNow.AddDays(1);
        var userId = _fixture.Create<Guid>();
        var bulletinsConfigurationOptions = _fixture.Create<BulletinsConfigurationOptions>();

        var configurationOptionsMock = new Mock<IOptions<BulletinsConfigurationOptions>>(MockBehavior.Strict);
        configurationOptionsMock
            .Setup(c => c.Value)
            .Returns(bulletinsConfigurationOptions);

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.CreateAsync(
                It.IsAny<Bulletin>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        bulletinRepositoryMock
            .Setup(r => r.GetUserBulletinsCountAsync(
                It.Is<Guid>(i => i == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletinsConfigurationOptions.MaxBulletinsCountPerUser + 1);

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Bulletins)
            .Returns(bulletinRepositoryMock.Object);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var imageServiceMock = new Mock<IImageService>();

        var request = new CreateBulletinCommand(text, rating, expiryUtc, userId, imageStream, imageExtension);
        var handler = new CreateBulletinCommandHandler(
            tenantFactoryMock.Object,
            imageServiceMock.Object,
            configurationOptionsMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action
            .Should()
            .ThrowAsync<LimitReachedException>();
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var tenantFactoryMock = new Mock<ITenantFactory>();
        var imageServiceMock = new Mock<IImageService>();
        CreateBulletinCommand request = null!;
        var configurationOptionsMock = new Mock<IOptions<BulletinsConfigurationOptions>>();
        var handler = new CreateBulletinCommandHandler(
            tenantFactoryMock.Object,
            imageServiceMock.Object,
            configurationOptionsMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}