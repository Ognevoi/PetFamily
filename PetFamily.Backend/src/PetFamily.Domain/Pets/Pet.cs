using CSharpFunctionalExtensions;
using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Pets;

public class Pet : Entity
{
    private readonly List<AssistanceDetails> _assistanceDetails = [];
    
    private Pet() { } // required by EF Core

    private Pet(
        string name,
        Species species,
        Breed breed,
        string description,
        string color,
        string healthInfo,
        decimal weight,
        decimal height,
        bool isSterilized,
        bool isVaccinated,
        DateTime birthDate,
        PetStatus petStatus,
        List<AssistanceDetails> assistanceDetails,
        string address,
        string phoneNumber
        )
    {
        Id = Guid.NewGuid();
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
        Address = address;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Species Species { get; private set; }
    public Breed Breed { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    public string HealthInfo { get; private set; }
    public decimal Weight { get; private set; }
    public decimal Height { get; private set; }
    public bool IsSterilized { get; private set; }
    public bool IsVaccinated { get; private set; }
    public DateTime BirthDate { get; private set; }
    public PetStatus PetStatus { get; private set; }
    
    public IReadOnlyList<AssistanceDetails> AssistanceDetails => _assistanceDetails;
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    
    public static Result<Pet> Create(
        string name,
        Species species,
        Breed breed,
        string description,
        string color,
        string healthInfo,
        decimal weight,
        decimal height,
        bool isSterilized,
        bool isVaccinated,
        DateTime birthDate,
        PetStatus petStatus,
        List<AssistanceDetails> assistanceDetails,
        string address,
        string phoneNumber)
    {

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet>("Name is required");
        
        if (species == null)
            return Result.Failure<Pet>("Species is required");

        if (breed == null)
            return Result.Failure<Pet>("Breed is required");

        if (phoneNumber == null)
            return Result.Failure<Pet>("Owner phone number is required");

        var pet = new Pet(
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
            petStatus,
            assistanceDetails,
            address,
            phoneNumber);
        
        return Result.Success(pet);
    }
}