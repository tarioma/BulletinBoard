using AutoMapper;
using BulletinBoard.Application.Bulletins.CreateBulletin;
using BulletinBoard.Application.Bulletins.SearchBulletins;
using BulletinBoard.Application.Bulletins.UpdateBulletin;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Contracts.Bulletins.Requests;
using BulletinBoard.Contracts.Bulletins.Responses;
using BulletinBoard.Domain.Entities;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class BulletinMappingProfile : Profile
{
    public BulletinMappingProfile()
    {
        CreateMap<CreateBulletinRequest, CreateBulletinCommand>()
            .ForCtorParam("ExpiryUtc", opt => opt.MapFrom(src => src.Expiry.UtcDateTime))
            .ForCtorParam("ImageStream",
                opt => opt.MapFrom(src => src.Image != null ? src.Image.OpenReadStream() : null))
            .ForCtorParam("ImageExtension",
                opt => opt.MapFrom(src => src.Image != null ? Path.GetExtension(src.Image.FileName) : null));

        CreateMap<UpdateBulletinRequest, UpdateBulletinCommand>()
            .ForCtorParam("ExpiryUtc", opt => opt.MapFrom(src => src.Expiry.UtcDateTime))
            .ForCtorParam("ImageStream",
                opt => opt.MapFrom(src => src.Image != null ? src.Image.OpenReadStream() : null))
            .ForCtorParam("ImageExtension",
                opt => opt.MapFrom(src => src.Image != null ? Path.GetExtension(src.Image.FileName) : null));

        CreateMap<Bulletin, GetBulletinByIdResponse>()
            .ForCtorParam("ImagePreview", opt => opt.MapFrom(src => src.Image == null
                ? null
                : $"images/preview/{src.Image}"))
            .ForCtorParam("ImageFull", opt => opt.MapFrom(src => src.Image == null
                ? null
                : $"images/full/{src.Image}"));

        CreateMap<SearchBulletinsRequest, BulletinsSearchFilters>()
            .ForCtorParam("CreatedFromUtc", opt =>
                opt.MapFrom(src => src.CreatedFrom.HasValue
                    ? src.CreatedFrom.Value.UtcDateTime
                    : (DateTime?)null))
            .ForCtorParam("CreatedToUtc", opt =>
                opt.MapFrom(src => src.CreatedTo.HasValue
                    ? src.CreatedTo.Value.UtcDateTime
                    : (DateTime?)null))
            .ForCtorParam("ExpiryFromUtc", opt =>
                opt.MapFrom(src => src.ExpiryFrom.HasValue
                    ? src.ExpiryFrom.Value.UtcDateTime
                    : (DateTime?)null))
            .ForCtorParam("ExpiryToUtc", opt =>
                opt.MapFrom(src => src.ExpiryTo.HasValue
                    ? src.ExpiryTo.Value.UtcDateTime
                    : (DateTime?)null));

        CreateMap<SearchBulletinsRequest, SearchBulletinsQuery>()
            .ConvertUsing((src, _, context) =>
            {
                var mapper = context.Mapper;
                var searchFilters = mapper.Map<BulletinsSearchFilters>(src);
                return new SearchBulletinsQuery(searchFilters);
            });

        CreateMap<IEnumerable<Bulletin>, SearchBulletinsResponse>()
            .ConvertUsing((src, _, context) =>
            {
                var bulletins = src.Select(u => context.Mapper.Map<GetBulletinByIdResponse>(u));
                return new SearchBulletinsResponse(bulletins);
            });
    }
}