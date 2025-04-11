using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public class Volunteer : SoftDeletableEntity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer() { } // required by EF Core

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        ExperienceYears experienceYears,
        PhoneNumber phoneNumber)
    {
        Id = volunteerId;
        FullName = fullName;
        Email = email;
        Description = description;
        ExperienceYears = experienceYears;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public ExperienceYears ExperienceYears { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public SocialNetworkList? SocialNetworksList { get; private set; }
    public AssistanceDetailsList? AssistanceDetailsList { get; private set; }
    public List<Pet> Pets => _pets;
    public DateTime CreatedAt { get; private set; }


    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        ExperienceYears experienceYears,
        PhoneNumber phoneNumber
    )
    {
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            description,
            experienceYears,
            phoneNumber
        );

        return volunteer;
    }

    public void Update(
        FullName fullName,
        Email email,
        Description description,
        ExperienceYears experienceYears,
        PhoneNumber phoneNumber
    )
    {
        FullName = fullName;
        Email = email;
        Description = description;
        ExperienceYears = experienceYears;
        PhoneNumber = phoneNumber;
    }

    public override void SoftDelete()
    {
        base.SoftDelete();

        foreach (var pet in _pets)
        {
            pet.SoftDelete();
        }
    }

    public override void Restore()
    {
        base.Restore();

        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public void CreateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks) =>
        SocialNetworksList = new SocialNetworkList(socialNetworks.ToList());

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks) =>
        SocialNetworksList = new SocialNetworkList(socialNetworks.ToList());

    public void CreateAssistanceDetails(IEnumerable<AssistanceDetails> assistanceDetails) =>
        AssistanceDetailsList = new AssistanceDetailsList(assistanceDetails.ToList());

    public void UpdateAssistanceDetails(IEnumerable<AssistanceDetails> assistanceDetails) =>
        AssistanceDetailsList = new AssistanceDetailsList(assistanceDetails.ToList());

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;

        pet.SetPosition(positionResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> RemovePet(Pet pet)
    {
        var petToRemove = _pets.FirstOrDefault(p => p.Id == pet.Id);
        if (petToRemove == null)
            return Errors.General.NotFound(pet.Id.Value);

        var removedPosition = petToRemove.Position.Value;

        _pets.Remove(petToRemove);

        // Shift positions of pets that were after the removed one
        var petsToUpdate = _pets
            .Where(p => p.Position.Value > removedPosition)
            .OrderBy(p => p.Position.Value)
            .ToList();

        foreach (var p in petsToUpdate)
        {
            var result = p.MoveBack();
            if (result.IsFailure)
                return result.Error;
        }

        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<Error>();

        var adjustedPositionResult = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPositionResult.IsFailure)
            return adjustedPositionResult.Error;

        newPosition = adjustedPositionResult.Value;

        var moveResult = MovePetBetweenPositions(newPosition, currentPosition);
        
        return moveResult.IsFailure ? moveResult.Error : UnitResult.Success<Error>();
    }
    
    private UnitResult<Error> MovePetBetweenPositions(Position newPosition, Position currentPosition)
    {
        // Find the pet that needs to be moved
        var petToMove = _pets.FirstOrDefault(p => p.Position == currentPosition);
        if (petToMove == null)
        {
            return Errors.General.NotFound(petToMove.Id.Value);
        }

        if (newPosition < currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position >= newPosition && p.Position < currentPosition)
                .ToList();

            foreach (var pet in petsToMove)
            {
                var result = pet.MoveForward();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position > currentPosition && p.Position <= newPosition)
                .ToList();

            foreach (var pet in petsToMove)
            {
                var result = pet.MoveBack();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }

        // Set the new position for the pet being moved
        petToMove.SetPosition(newPosition);
        
        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition;
    }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId);

        return pet;
    }
}