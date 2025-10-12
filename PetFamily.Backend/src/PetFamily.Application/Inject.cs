using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Behaviors;
using PetFamily.Application.Caching;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        var assembly = typeof(Inject).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddDistributedCache(configuration);

        return services;
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
