using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Application.Users.CreateUser;
using BulletinBoard.Application.Users.SearchUsers;
using BulletinBoard.Application.Users.UpdateUser;
using BulletinBoard.Contracts.Users.Requests;
using BulletinBoard.Contracts.Users.Responses;
using BulletinBoard.Domain.Entities;
using Mapster;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, CreateUserCommand>();
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>();
        config.NewConfig<User, GetUserByIdResponse>();

        config.NewConfig<SearchUsersRequest, UsersSearchFilters>()
            .Map(dest => dest.Page,
                src => new PageFilter(src.Count, src.Offset))
            .Map(dest => dest.SearchName, src => src.Name)
            .Map(dest => dest.SearchIsAdmin, src => src.IsAdmin)
            .Map(dest => dest.Created,
                src => new DateRangeFilters(
                    src.CreatedFrom.HasValue ? src.CreatedFrom.Value.UtcDateTime : null,
                    src.CreatedTo.HasValue ? src.CreatedTo.Value.UtcDateTime : null));

        config.NewConfig<SearchUsersRequest, SearchUsersQuery>()
            .MapWith(src => new SearchUsersQuery(src.Adapt<UsersSearchFilters>()));

        config.NewConfig<IEnumerable<User>, SearchUsersResponse>()
            .MapWith(src => new SearchUsersResponse(src.Select(u => u.Adapt<GetUserByIdResponse>())));
    }
}