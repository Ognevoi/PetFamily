using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.CreateSpecie;
using PetFamily.Application.Species.DeleteBreed;
using PetFamily.Application.Species.DeleteSpecie;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.DeletePetPhoto;
using PetFamily.Application.Volunteers.GetPetPhoto;
using PetFamily.Application.Volunteers.Restore;
using PetFamily.Application.Volunteers.Update;
using PetFamily.Application.Volunteers.UpdateVolunteerAssistanceDetails;
using PetFamily.Application.Volunteers.UpdateVolunteerSocialNetworks;
using PetFamily.Application.Volunteers.UploadPetPhoto;

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