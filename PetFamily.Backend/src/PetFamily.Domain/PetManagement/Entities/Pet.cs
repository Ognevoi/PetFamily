using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    private readonly List<Photo> _photos = [];

    public Volunteer Volunteer { get; private set; } = null!;

    private Pet()
    {
    } // required by EF Core

    public Pet(
        PetId petId,
        PetName name,
        Specie specie,
        Breed breed,
        Address address,
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
        Id = petId;
        Name = name;
        Specie = specie;
        Breed = breed;
        Address = address;
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
    public Specie Specie { get; private set; }
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
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Photo> Photos => _photos;
    public Position Position { get; private set; }
    
    public void AddPhotos(List<Photo> photos)
    {
        _photos.AddRange(photos);
    }

    public void RemovePhotos(List<Photo> photos)
    {
        foreach (var photo in photos)
        {
            _photos.Remove(photo);
        }
    }
    
    public void SetPosition(Position position)
    {
        Position = position;
    }
    
    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;
        
        return UnitResult.Success<Error>();
    }
}