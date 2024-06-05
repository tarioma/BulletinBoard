using BulletinBoard.Application.Bulletins.CreateBulletin;
using BulletinBoard.Application.Bulletins.SearchBulletins;
using BulletinBoard.Application.Bulletins.UpdateBulletin;
using BulletinBoard.Application.Models.Bulletins;
using BulletinBoard.Application.SearchFilters;
using BulletinBoard.Contracts.Bulletins.Requests;
using BulletinBoard.Contracts.Bulletins.Responses;
using BulletinBoard.Domain.Entities;
using Mapster;

namespace BulletinBoard.WebAPI.MappingProfiles;

public class BulletinMappingProfile : IRegister
{
    private const string PreviewImagesPath = "/images/preview/";
    private const string FullImagesPath = "/images/full/";

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBulletinRequest, CreateBulletinCommand>()
            .ConstructUsing(src => new CreateBulletinCommand(
                src.Text,
                src.Rating,
                src.Expiry.UtcDateTime,
                src.UserId,
                () => src.Image != null ? src.Image.OpenReadStream() : null,
                src.Image != null ? Path.GetExtension(src.Image.FileName) : null));

        config.NewConfig<UpdateBulletinRequest, UpdateBulletinCommand>()
            .ConstructUsing(src => new UpdateBulletinCommand(
                Guid.Empty, // Задаётся в контроллере из параметров запроса
                src.Text,
                src.Rating,
                src.Expiry.UtcDateTime,
                () => src.Image != null ? src.Image.OpenReadStream() : null,
                src.Image != null ? Path.GetExtension(src.Image.FileName) : null));

        config.NewConfig<Bulletin, GetBulletinByIdResponse>()
            .Map(dest => dest.ImagePreview,
                src => src.Image == null ? null : PreviewImagesPath + src.Image)
            .Map(dest => dest.ImageFull,
                src => src.Image == null ? null : FullImagesPath + src.Image);

        config.NewConfig<SearchBulletinsRequest, BulletinsSearchFilters>()
            .Map(dest => dest.Page,
                src => new PageFilter(src.Count, src.Offset))
            .Map(dest => dest.Rating,
                src => new BulletinsRatingFilter(src.RatingFrom, src.RatingTo))
            .Map(dest => dest.Created,
                src => new DateRangeFilters(
                    src.CreatedFrom.HasValue ? src.CreatedFrom.Value.UtcDateTime : null,
                    src.CreatedTo.HasValue ? src.CreatedTo.Value.UtcDateTime : null))
            .Map(dest => dest.Expiry,
                src => new DateRangeFilters(
                    src.ExpiryFrom.HasValue ? src.ExpiryFrom.Value.UtcDateTime : null,
                    src.ExpiryTo.HasValue ? src.ExpiryTo.Value.UtcDateTime : null));

        config.NewConfig<SearchBulletinsRequest, SearchBulletinsQuery>()
            .MapWith(src => new SearchBulletinsQuery(src.Adapt<BulletinsSearchFilters>()));

        config.NewConfig<IEnumerable<Bulletin>, SearchBulletinsResponse>()
            .MapWith(src => new SearchBulletinsResponse(src.Select(u => u.Adapt<GetBulletinByIdResponse>())));
    }
}