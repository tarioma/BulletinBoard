using AutoFixture;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Domain.Tests.Tools;

public class BulletinCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var id = fixture.Create<Guid>();
        var number = fixture.Create<int>();
        var text = fixture.Create<string>();
        var rating = fixture.Create<int>();
        var createdUtc = fixture.Create<DateTime>();
        var expiryUtc = createdUtc.AddDays(1);
        var userId = fixture.Create<Guid>();

        fixture.Customize<Bulletin>(composer =>
            composer.FromFactory(() =>
                new Bulletin(id, number, text, rating, createdUtc, expiryUtc, userId)));
    }
}