using AutoFixture;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.Application.Tests.Customizations;

public class UsersSearchFiltersCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var page = fixture.Create<PageFilter>();
        var searchName = fixture.Create<string?>();
        var searchIsAdmin = fixture.Create<bool?>();
        const string sortBy = nameof(User.CreatedUtc);
        var desc = fixture.Create<bool>();
        var created = fixture.Create<DateRangeFilters>();

        fixture.Customize<UsersSearchFilters>(composer =>
            composer.FromFactory(() =>
                new UsersSearchFilters(page, searchName, searchIsAdmin, sortBy, desc, created)));
    }
}