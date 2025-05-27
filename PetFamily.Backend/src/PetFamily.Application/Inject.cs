using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(typeof(Inject).Assembly)
            .AddCommands();
        
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
}