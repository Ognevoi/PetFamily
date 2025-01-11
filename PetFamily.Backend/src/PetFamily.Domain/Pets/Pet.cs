using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Pets;

public class Pet : Shared.Entity<PetId>
{
    
    private readonly List<AssistanceDetails> _assistanceDetails = [];
    private Pet(PetId id) : base(id) { } // required by EF Core

    private Pet(
        PetId petId,
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
        string address,
        string phoneNumber
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
        Address = address;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

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
    public PetPhoto? PetPhoto { get; private set; }

    
    public static Result<Pet> Create(
        PetId petId,
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
        string address,
        string phoneNumber)
    {

        if (string.IsNullOrWhiteSpace(name))
            return "Name is required";
        
        if (species == null)
            return "Species is required";

        if (breed == null)
            return "Breed is required";

        if (phoneNumber == null)
            return "Owner phone number is required";

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
            petStatus,
            address,
            phoneNumber);
        
        return pet;
    }
    
    public Result AddPhoto(string url, string fileName)
    {
        if (PetPhoto != null)
            return "Photo already exists. Remove the existing photo to add a new one.";

        var photoResult = PetPhoto.Create(url);
        
        if (photoResult.IsFailure)
            return photoResult.Error;

        PetPhoto = photoResult.Value;
        return Result.Success();
    }

    public Result RemovePhoto()
    {
        if (PetPhoto == null)
            return "No photo exists to remove.";

        PetPhoto = null;
        return Result.Success();
    }
}