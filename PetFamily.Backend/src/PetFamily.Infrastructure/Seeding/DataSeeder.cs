using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Seeding;

public class DataSeeder : ISeeder
{
    private readonly WriteDbContext _dbContext;
    private readonly ILogger<DataSeeder> _logger;
    private readonly Random _random = new();

    public DataSeeder(WriteDbContext dbContext, ILogger<DataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting database seeding...");

        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            // Clear existing data
            await ClearDatabaseAsync();
            
            // Seed data
            await SeedDataAsync();
            
            await transaction.CommitAsync();
            
            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database seeding.");
            throw;
        }
    }

    private async Task ClearDatabaseAsync()
    {
        _logger.LogInformation("Clearing existing data...");
        
        // Clear in order to respect foreign key constraints
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM pets");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM volunteers");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM breeds");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM species");
        
        _logger.LogInformation("Database cleared successfully.");
    }

    private async Task SeedDataAsync()
    {
        _logger.LogInformation("Seeding species and breeds...");
        var species = await SeedSpeciesAndBreedsAsync();
        
        _logger.LogInformation("Seeding volunteers...");
        var volunteers = await SeedVolunteersAsync();
        
        _logger.LogInformation("Seeding pets...");
        await SeedPetsAsync(volunteers, species);
        
        _logger.LogInformation("All data seeded successfully.");
    }

    private async Task<List<Specie>> SeedSpeciesAndBreedsAsync()
    {
        var species = new List<Specie>();
        var breeds = new List<Breed>();
        
        for (int i = 0; i < SeedingConstants.SPECIES_COUNT; i++)
        {
            var specieId = SpecieId.Create(Guid.NewGuid());
            var specieName = SeedingConstants.SpeciesNames[i % SeedingConstants.SpeciesNames.Length];
            
            var specieResult = Specie.Create(specieId, specieName);
            if (specieResult.IsSuccess)
            {
                var specie = specieResult.Value;
                species.Add(specie);
                
                // Add breeds for this species
                var breedsCount = _random.Next(SeedingConstants.BREEDS_PER_SPECIES_MIN, SeedingConstants.BREEDS_PER_SPECIES_MAX + 1);
                var breedNames = SeedingConstants.BreedNames[i % SeedingConstants.BreedNames.Length];
                
                for (int j = 0; j < breedsCount; j++)
                {
                    var breedId = BreedId.Create(Guid.NewGuid());
                    var breedName = breedNames[j % breedNames.Length];
                    
                    var breedResult = Breed.Create(breedId, breedName);
                    if (breedResult.IsSuccess)
                    {
                        var breed = breedResult.Value;
                        breeds.Add(breed);
                        specie.AddBreed(breed);
                    }
                }
            }
        }
        
        // Save species and breeds in batches
        await _dbContext.Species.AddRangeAsync(species);
        await _dbContext.SaveChangesAsync();
        
        return species;
    }

    private async Task<List<Volunteer>> SeedVolunteersAsync()
    {
        var volunteers = new List<Volunteer>();
        
        for (int i = 0; i < SeedingConstants.VOLUNTEERS_COUNT; i++)
        {
            var volunteerId = VolunteerId.NewVolunteerId();
            var firstName = SeedingConstants.FirstNames[_random.Next(SeedingConstants.FirstNames.Length)];
            var lastName = SeedingConstants.LastNames[_random.Next(SeedingConstants.LastNames.Length)];
            
            var fullNameResult = FullName.Create(firstName, lastName);
            if (fullNameResult.IsFailure) continue;
            
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";
            var emailResult = Email.Create(email);
            if (emailResult.IsFailure) continue;
            
            var description = GenerateRandomDescription();
            var descriptionResult = Description.Create(description);
            if (descriptionResult.IsFailure) continue;
            
            var experienceYears = ExperienceYears.Create(_random.Next(0, 20));
            if (experienceYears.IsFailure) continue;
            
            var phoneNumber = GenerateRandomPhoneNumber();
            var phoneResult = PhoneNumber.Create(phoneNumber);
            if (phoneResult.IsFailure) continue;
            
            var volunteerResult = Volunteer.Create(
                volunteerId,
                fullNameResult.Value,
                emailResult.Value,
                descriptionResult.Value,
                experienceYears.Value,
                phoneResult.Value
            );
            
            if (volunteerResult.IsSuccess)
            {
                var volunteer = volunteerResult.Value;
                
                // Add social networks
                if (_random.NextDouble() > 0.3) // 70% chance
                {
                    var socialNetworks = GenerateSocialNetworks();
                    volunteer.CreateSocialNetworks(socialNetworks);
                }
                
                // Add assistance details
                if (_random.NextDouble() > 0.4) // 60% chance
                {
                    var assistanceDetails = GenerateAssistanceDetails();
                    volunteer.CreateAssistanceDetails(assistanceDetails);
                }
                
                volunteers.Add(volunteer);
            }
        }
        
        // Save volunteers in batches
        for (int i = 0; i < volunteers.Count; i += SeedingConstants.BATCH_SIZE)
        {
            var batch = volunteers.Skip(i).Take(SeedingConstants.BATCH_SIZE);
            await _dbContext.Volunteers.AddRangeAsync(batch);
            await _dbContext.SaveChangesAsync();
        }
        
        return volunteers;
    }

    private async Task SeedPetsAsync(List<Volunteer> volunteers, List<Specie> species)
    {
        foreach (var volunteer in volunteers)
        {
            var petsCount = _random.Next(SeedingConstants.PETS_PER_VOLUNTEER_MIN, SeedingConstants.PETS_PER_VOLUNTEER_MAX + 1);
            
            for (int i = 0; i < petsCount; i++)
            {
                // Reload volunteer from database to get current pet count
                var currentVolunteer = await _dbContext.Volunteers
                    .Include(v => v.Pets)
                    .FirstOrDefaultAsync(v => v.Id == volunteer.Id);
                
                if (currentVolunteer == null)
                {
                    _logger.LogWarning("Volunteer {VolunteerId} not found in database", volunteer.Id);
                    continue;
                }
                
                var pet = await CreateRandomPetAsync(currentVolunteer, species);
                if (pet != null)
                {
                    var addPetResult = currentVolunteer.AddPet(pet);
                    if (addPetResult.IsFailure)
                    {
                        _logger.LogWarning("Failed to add pet to volunteer {VolunteerId}: {Error}", currentVolunteer.Id, addPetResult.Error);
                    }
                    else
                    {
                        // Save after each pet to ensure proper position tracking
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }

    private async Task<Pet?> CreateRandomPetAsync(Volunteer volunteer, List<Specie> species)
    {
        try
        {
            var petId = PetId.Create(Guid.NewGuid());
            var petName = SeedingConstants.PetNames[_random.Next(SeedingConstants.PetNames.Length)];
            var petNameResult = PetName.Create(petName);
            if (petNameResult.IsFailure) return null;
            
            var specie = species[_random.Next(species.Count)];
            var breed = specie.Breed[_random.Next(specie.Breed.Count)];
            
            var address = GenerateRandomAddress();
            var addressResult = Address.Create(address.Street, address.City, address.State, address.ZipCode);
            if (addressResult.IsFailure) return null;
            
            var description = _random.NextDouble() > 0.3 ? GenerateRandomDescription() : null;
            var descriptionResult = description != null ? Description.Create(description) : Result.Success<Description, Error>(null);
            if (descriptionResult.IsFailure) return null;
            
            var color = SeedingConstants.PetColors[_random.Next(SeedingConstants.PetColors.Length)];
            var colorResult = PetColor.Create(color);
            if (colorResult.IsFailure) return null;
            
            var healthInfo = SeedingConstants.HealthConditions[_random.Next(SeedingConstants.HealthConditions.Length)];
            var healthResult = PetHealthInfo.Create(healthInfo);
            if (healthResult.IsFailure) return null;
            
            var weight = Weight.Create(_random.NextDouble() * 50 + 1); // 1-51 kg
            if (weight.IsFailure) return null;
            
            var height = Height.Create(_random.NextDouble() * 100 + 10); // 10-110 cm
            if (height.IsFailure) return null;
            
            var isSterilized = IsSterilized.Create(_random.NextDouble() > 0.5);
            var isVaccinated = IsVaccinated.Create(_random.NextDouble() > 0.3);
            
            var birthDate = DateTime.UtcNow.AddDays(-_random.Next(365 * 15)); // Up to 15 years old
            var petStatus = (PetStatus)_random.Next(Enum.GetValues<PetStatus>().Length);
            
            var pet = new Pet(
                petId,
                petNameResult.Value,
                specie,
                breed,
                addressResult.Value,
                descriptionResult.Value,
                colorResult.Value,
                healthResult.Value,
                weight.Value,
                height.Value,
                isSterilized.Value,
                isVaccinated.Value,
                birthDate,
                petStatus
            );
            
            return pet;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create pet for volunteer {VolunteerId}", volunteer.Id);
            return null;
        }
    }

    private string GenerateRandomDescription()
    {
        var descriptions = new[]
        {
            "Friendly and playful pet looking for a loving home.",
            "Calm and gentle companion, great with children.",
            "Energetic and active, needs regular exercise.",
            "Older pet with lots of love to give.",
            "Young and curious, ready for adventures.",
            "Quiet and well-behaved, perfect for apartment living.",
            "Social and outgoing, loves meeting new people.",
            "Independent but affectionate when needed.",
            "Trained and well-mannered, great family pet.",
            "Special needs pet requiring extra care and attention."
        };
        
        return descriptions[_random.Next(descriptions.Length)];
    }

    private (string Street, string City, string State, string ZipCode) GenerateRandomAddress()
    {
        var streetNumber = _random.Next(1, 9999);
        var street = $"{streetNumber} {SeedingConstants.Streets[_random.Next(SeedingConstants.Streets.Length)]}";
        var city = SeedingConstants.Cities[_random.Next(SeedingConstants.Cities.Length)];
        var state = SeedingConstants.States[_random.Next(SeedingConstants.States.Length)];
        var zipCode = _random.Next(10000, 99999).ToString();
        
        return (street, city, state, zipCode);
    }

    private string GenerateRandomPhoneNumber()
    {
        var firstDigit = 5; 
        var rest = _random.Next(100000, 999999); // ensures 6 more digits
        return $"+372 {firstDigit}{rest}";
    }

    private List<SocialNetwork> GenerateSocialNetworks()
    {
        var count = _random.Next(1, 4); // 1-3 social networks
        var socialNetworks = new List<SocialNetwork>();
        
        for (int i = 0; i < count; i++)
        {
            var type = SeedingConstants.SocialNetworkTypes[_random.Next(SeedingConstants.SocialNetworkTypes.Length)];
            var url = $"https://{type.ToLower()}.com/user{_random.Next(1000, 9999)}";
            
            var socialNetworkResult = SocialNetwork.Create(type, url);
            if (socialNetworkResult.IsSuccess)
            {
                socialNetworks.Add(socialNetworkResult.Value);
            }
        }
        
        return socialNetworks;
    }

    private List<AssistanceDetails> GenerateAssistanceDetails()
    {
        var count = _random.Next(1, 4); // 1-3 assistance types
        var assistanceDetails = new List<AssistanceDetails>();
        
        for (int i = 0; i < count; i++)
        {
            var type = SeedingConstants.AssistanceTypes[_random.Next(SeedingConstants.AssistanceTypes.Length)];
            var description = $"Can provide {type.ToLower()} for pets in need";
            
            var assistanceResult = AssistanceDetails.Create(type, description);
            if (assistanceResult.IsSuccess)
            {
                assistanceDetails.Add(assistanceResult.Value);
            }
        }
        
        return assistanceDetails;
    }
    
}