using AutoFixture;
using BulletinBoard.Application.Bulletins.GetBulletinById;
using BulletinBoard.Application.Repositories;
using BulletinBoard.Application.Tests.Extensions;
using BulletinBoard.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BulletinBoard.Application.Tests.Bulletins.GetBulletinById;

public class GetBulletinByIdQueryHandlerTests
{
    private readonly IFixture _fixture = ApplicationFixtureExtensions.GetFixtureWithAllCustomizations();

    [Fact]
    public async Task Handle_ValidRequest_Successfully()
    {
        // Arrange
        var bulletin = _fixture.Create<Bulletin>();

        var bulletinRepositoryMock = new Mock<IBulletinRepository>(MockBehavior.Strict);
        bulletinRepositoryMock
            .Setup(r => r.GetByIdAsync(
                It.Is<Guid>(i => i == bulletin.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletin);

        var tenantMock = new Mock<ITenant>(MockBehavior.Strict);
        tenantMock
            .SetupGet(t => t.Bulletins)
            .Returns(bulletinRepositoryMock.Object);

        var tenantFactoryMock = new Mock<ITenantFactory>(MockBehavior.Strict);
        tenantFactoryMock
            .Setup(f => f.GetTenant())
            .Returns(tenantMock.Object);

        var request = new GetBulletinByIdQuery(bulletin.Id);
        var handler = new GetBulletinByIdQueryHandler(tenantFactoryMock.Object);

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
        GetBulletinByIdQuery request = null!;
        var handler = new GetBulletinByIdQueryHandler(tenantFactoryMock.Object);

        // Act
        var action = async () => await handler.Handle(request);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(nameof(request));
    }
}