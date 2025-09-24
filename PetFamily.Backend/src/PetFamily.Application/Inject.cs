using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Behaviors;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        var assembly = typeof(Inject).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}