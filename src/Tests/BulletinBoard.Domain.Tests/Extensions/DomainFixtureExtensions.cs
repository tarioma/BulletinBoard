using AutoFixture;
using BulletinBoard.Domain.Tests.Customizations;

namespace BulletinBoard.Domain.Tests.Extensions;

public static class DomainFixtureExtensions
{
    public static IFixture GetFixtureWithAllCustomizations()
    {
        var fixture = new Fixture();
        fixture.Customize(new BulletinCustomization());

        return fixture;
    }

    public static string CreateString(this IFixture fixture, int length) =>
        new(fixture.CreateMany<char>(length).ToArray());
}