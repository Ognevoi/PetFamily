using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Caching;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidatorsFromAssembly(typeof(Inject).Assembly)
            .AddCommands()
            .AddQueries()
            .AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }

    public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            string connection = configuration.GetConnectionString("Redis") ??
                                throw new ArgumentNullException(nameof(connection));

            options.Configuration = connection;
        });
        
        services.AddSingleton<ICacheService, DistributedCacheService>();

        return services;
    }
}