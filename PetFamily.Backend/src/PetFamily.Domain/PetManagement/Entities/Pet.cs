using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.Entities;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Shared.Entity<PetId>
{
    private Pet(PetId id) : base(id) { } // required by EF Core

    private Pet(
        PetId petId,
        PetName name,
        Species species,
        Breed breed,
        Description? description,
        PetColor color,
        PetHealthInfo healthInfo,
        Weight weight,
        Height height,
        IsSterilized isSterilized,
        IsVaccinated isVaccinated,
        DateTime birthDate,
        PetStatus petStatus
        ) : base(petId)
    {
        Name = name;
        Species = species;
        Breed = breed;
        Description = description;
        Color = color;
        HealthInfo = healthInfo;
        Weight = weight;
        Height = height;
        IsSterilized = isSterilized;
        IsVaccinated = isVaccinated;
        BirthDate = birthDate;
        PetStatus = petStatus;
        CreatedAt = DateTime.UtcNow;
    }

    public PetName Name { get; private set; }
    public Species Species { get; private set; }
    public Breed Breed { get; private set; }
    public Description? Description { get; private set; }
    public PetColor Color { get; private set; }
    public PetHealthInfo HealthInfo { get; private set; }
    public Weight Weight { get; private set; }
    public Height Height { get; private set; }
    public IsSterilized IsSterilized { get; private set; }
    public IsVaccinated IsVaccinated { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public PetStatus PetStatus { get; private set; }
    public AssistanceDetailsList? AssistanceDetails { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public PetPhoto? PetPhoto { get; private set; }

    
    public static Result<Pet> Create(
        PetId petId,
        PetName name,
        Species species,
        Breed breed,
        Description? description,
        PetColor color,
        PetHealthInfo healthInfo,
        Weight weight,
        Height height,
        IsSterilized isSterilized,
        IsVaccinated isVaccinated,
        DateTime birthDate,
        PetStatus petStatus)
    {
        var pet = new Pet(
            petId,
            name,
            species,
            breed,
            description,
            color,
            healthInfo,
            weight,
            height,
            isSterilized,
            isVaccinated,
            birthDate,
            petStatus);
        
        return pet;
    }
    
    public Result<PetPhoto, Error> AddPhoto(string url, string fileName)
    {
        if (PetPhoto != null)
            return Errors.General.ValueAlreadyExists("Photo");

        var photoResult = PetPhoto.Create(url);
        
        if (photoResult.IsFailure)
            return photoResult.Error;

        PetPhoto = photoResult.Value;
        
        return PetPhoto;
    }

    public Result<PetPhoto, Error> RemovePhoto()
    {
        if (PetPhoto == null)
            return Errors.General.NotFound();

        PetPhoto = null;
        
        return PetPhoto;
    }
}