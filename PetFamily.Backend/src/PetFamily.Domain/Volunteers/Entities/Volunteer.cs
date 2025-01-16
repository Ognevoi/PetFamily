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

    
    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        string fullName,
        Email email,
        string description,
        int experienceYears,
        PhoneNumber phoneNumber
    )
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "Full name is required";
        
        if (string.IsNullOrWhiteSpace(description))
            return "Description is required";
        
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
    
    // TODO: review this code later for correctness
    public void CreateSocialNetworks(string name, string url)
    {
        var result = SocialNetwork.Create(name, url);

        SocialNetworksList = new SocialNetworkList(new List<SocialNetwork> { result.Value });
    }
    
    // TODO: review this code later for correctness
    public void CreateAssistanceDetails(string name, string description)
    {
        var result = AssistanceDetails.Create(name, description);
        
        AssistanceDetailsList = new AssistanceDetailsList(new List<AssistanceDetails> { result.Value });
    }
    
}
    

    