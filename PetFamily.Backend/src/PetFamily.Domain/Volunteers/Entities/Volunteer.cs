using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Volunteers.Entities;

    public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    
    private Volunteer(VolunteerId id) : base(id) { } // required by EF Core

    private Volunteer(
        VolunteerId volunteerId,
        string fullName,
        Email email,
        string description,
        int experienceYears,
        PhoneNumber phoneNumber
    ) : base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        ExperienceYears = experienceYears;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string Description { get; private set; }
    public int ExperienceYears { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public SocialNetworkList? SocialNetworksList { get; private set; }
    public AssistanceDetailsList? AssistanceDetailsList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    public DateTime CreatedAt { get; private set; }

    
    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        string fullName,
        Email email,
        string description,
        int experienceYears,
        PhoneNumber phoneNumber
        )
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Errors.General.ValueIsRequired("Full name");
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired("Description");
        
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
    
    public void CreateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworksList = new SocialNetworkList(socialNetworks.ToList());
    }

    public void CreateAssistanceDetails(IEnumerable<AssistanceDetails> assistanceDetails)
    {
        AssistanceDetailsList = new AssistanceDetailsList(assistanceDetails.ToList());
    }
}