using AutoFixture;

namespace BulletinBoard.Domain.Tests.Tools;

public static class FixtureExtensions
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