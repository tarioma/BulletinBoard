using AutoMapper;
using BulletinBoard.Application.Models.Users;
using BulletinBoard.Application.Users.CreateUser;
using BulletinBoard.Application.Users.SearchUsers;
using BulletinBoard.Application.Users.UpdateUser;
using BulletinBoard.Contracts.Users.Requests;
using BulletinBoard.Contracts.Users.Responses;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();

        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<User, GetUserByIdResponse>();

        CreateMap<SearchUsersRequest, UsersSearchFilters>()
            .ForCtorParam("CreatedFromUtc", opt =>
                opt.MapFrom(src => src.CreatedFrom.HasValue
                    ? src.CreatedFrom.Value.UtcDateTime
                    : (DateTime?)null))
            .ForCtorParam("CreatedToUtc", opt =>
                opt.MapFrom(src => src.CreatedTo.HasValue
                    ? src.CreatedTo.Value.UtcDateTime
                    : (DateTime?)null));

        CreateMap<SearchUsersRequest, SearchUsersQuery>()
            .ConvertUsing((src, _, context) =>
            {
                var mapper = context.Mapper;
                var searchFilters = mapper.Map<UsersSearchFilters>(src);
                return new SearchUsersQuery(searchFilters);
            });

        CreateMap<IEnumerable<User>, SearchUsersResponse>()
            .ConvertUsing((src, _, context) =>
            {
                var users = src.Select(u => context.Mapper.Map<GetUserByIdResponse>(u));
                return new SearchUsersResponse(users);
            });
    }
}