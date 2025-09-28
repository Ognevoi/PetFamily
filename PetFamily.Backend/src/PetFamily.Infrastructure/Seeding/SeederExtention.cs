using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Infrastructure.Seeding;

public static class SeederExtention
{
    public static async Task<IServiceProvider> RunSeeding(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        
        var seeders = scope.ServiceProvider.GetServices<ISeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
        
        return services;
    }
    
}