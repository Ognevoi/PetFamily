using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Volunteers;

public class Volunteer : Entity
{
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<AssistanceDetails> _assistanceDetails = [];
    private readonly List<Pet> _pets = [];
    
    private Volunteer() { } // required by EF Core

    private Volunteer(
        string fullName,
        string email,
        string description,
        int experienceYears,
        string phoneNumber
    )
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Description = description;
        ExperienceYears = experienceYears;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Description { get; private set; }
    public int ExperienceYears { get; private set; }
    public string PhoneNumber { get; private set; }
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<AssistanceDetails> AssistanceDetails => _assistanceDetails;
    public IReadOnlyList<Pet> Pets => _pets;
    public DateTime CreatedAt { get; private set; }

    
    public static Result<Volunteer> Create(
        string fullName,
        string email,
        string description,
        int experienceYears,
        string phoneNumber
    )
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<Volunteer>("Full name is required");
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Description is required");
        
        var volunteer = new Volunteer(
            fullName,
            email,
            description,
            experienceYears,
            phoneNumber
        );
        
        return Result.Success(volunteer);
    }

}
    

    