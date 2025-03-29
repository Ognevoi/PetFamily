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

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }
    
    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId);

        return pet;
    }

}