using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

    public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    
    private Volunteer(VolunteerId id) : base(id) { } // required by EF Core

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        ExperienceYears experienceYears,
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

    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public ExperienceYears ExperienceYears { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public SocialNetworkList? SocialNetworksList { get; private set; }
    public AssistanceDetailsList? AssistanceDetailsList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
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
    
    public void CreateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworksList = new SocialNetworkList(socialNetworks.ToList());
    }

    public void CreateAssistanceDetails(IEnumerable<AssistanceDetails> assistanceDetails)
    {
        AssistanceDetailsList = new AssistanceDetailsList(assistanceDetails.ToList());
    }
}