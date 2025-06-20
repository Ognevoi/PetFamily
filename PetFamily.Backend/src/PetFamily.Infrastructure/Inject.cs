using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddMinio(configuration)
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices()
            .AddMessageQueues();
        
        return services;    
    }
    
    private static IServiceCollection AddMessageQueues(this IServiceCollection services)
    {
        services.AddSingleton<IFileCleanerQueue, FileCleanerQueue>();
        
        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        return services;
    }
        
    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Minio options not found");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.SSL);

        });
        
        services.AddScoped<IFilesProvider, MinioProvider>();

        return services;
    }
}