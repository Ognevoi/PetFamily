using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Species.AddBreed;
using PetFamily.Application.Features.Species.CreateSpecie;
using PetFamily.Application.Features.Species.DeleteBreed;
using PetFamily.Application.Features.Species.DeleteSpecie;
using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Application.Features.Volunteers.Create;
using PetFamily.Application.Features.Volunteers.DeletePetPhoto;
using PetFamily.Application.Features.Volunteers.GetPetPhoto;
using PetFamily.Application.Features.Volunteers.HardDelete;
using PetFamily.Application.Features.Volunteers.HardDeletePet;
using PetFamily.Application.Features.Volunteers.Restore;
using PetFamily.Application.Features.Volunteers.SoftDelete;
using PetFamily.Application.Features.Volunteers.Update;
using PetFamily.Application.Features.Volunteers.UpdatePetPosition;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;
using PetFamily.Application.Features.Volunteers.UploadPetPhoto;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerSocialNetworksHandler>();
        services.AddScoped<UpdateVolunteerAssistanceDetailsHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<RestoreVolunteerHandler>();
        services.AddScoped<HardDeletePetHandler>();
        services.AddScoped<UpdatePetPositionHandler>();
        services.AddScoped<UploadPetPhotosHandler>();
        services.AddScoped<DeletePetPhotosHandler>();
        services.AddScoped<GetPetPhotosHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<CreateSpecieHandler>();
        services.AddScoped<AddBreedHandler>();
        services.AddScoped<DeleteSpecieHandler>();
        services.AddScoped<DeleteBreedHandler>();
        return services;
    }
}