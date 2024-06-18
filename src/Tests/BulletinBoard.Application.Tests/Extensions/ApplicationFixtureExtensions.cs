using AutoFixture;
using BulletinBoard.Application.Tests.Customizations;
using BulletinBoard.Domain.Tests.Extensions;

namespace BulletinBoard.Application.Tests.Extensions;

public static class ApplicationFixtureExtensions
{
    public static IFixture GetFixtureWithAllCustomizations()
    {
        var fixture = DomainFixtureExtensions.GetFixtureWithAllCustomizations();
        fixture.Customize(new UsersSearchFiltersCustomization());
        fixture.Customize(new BulletinsSearchFiltersCustomization());

        return fixture;
    }
}