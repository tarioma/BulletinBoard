﻿using Microsoft.Extensions.DependencyInjection;

namespace Users.Application.Extensions;

public static class MicrosoftDependencyInjectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }
}