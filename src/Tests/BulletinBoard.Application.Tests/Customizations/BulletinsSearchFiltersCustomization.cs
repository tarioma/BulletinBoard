using AutoFixture;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Tests.Customizations;

public class BulletinsSearchFiltersCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var page = fixture.Create<PageFilter>();
        var searchNumber = fixture.Create<int?>();
        var searchText = fixture.Create<string?>();
        var searchUserId = fixture.Create<Guid?>();
        const string sortBy = nameof(User.CreatedUtc);
        var desc = fixture.Create<bool>();
        var rating = fixture.Create<BulletinsRatingFilter>();
        var created = fixture.Create<DateRangeFilters>();
        var expiry = fixture.Create<DateRangeFilters>();

        fixture.Customize<BulletinsSearchFilters>(composer =>
            composer.FromFactory(() =>
                new BulletinsSearchFilters(
                    page, searchNumber, searchText, searchUserId, sortBy, desc, rating, created, expiry)));
    }
}