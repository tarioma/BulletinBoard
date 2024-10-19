using System.Text.Json;
using System.Text.Json.Serialization;
using Bulletins.Application.Abstraction.Repositories;
using Bulletins.Dal;
using Bulletins.Dal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bulletins.Extensions;

public static class MicrosoftDependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBulletinRepository, BulletinRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection ApplyMigrations(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();

        return services;
    }
}