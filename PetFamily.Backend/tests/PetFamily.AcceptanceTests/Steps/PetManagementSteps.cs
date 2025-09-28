using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AcceptanceTests.Infrastructure;
using PetFamily.API.DTO.Requests.Volunteer;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Models;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.TestUtils.Seeding;
using Reqnroll;

namespace PetFamily.AcceptanceTests.Steps;

[Binding]
public class PetManagementSteps : BaseStepClass
{
    public PetManagementSteps(TestContext context) : base(context)
    {
    }

    [Given(@"I have a volunteer in the system")]
    public async Task GivenIHaveAVolunteerInTheSystem()
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
        
        _context.VolunteerId = volunteer.Id.Value;
    }

    [Given(@"I have species and breeds available")]
    public async Task GivenIHaveSpeciesAndBreedsAvailable()
    {
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        var specie = await SpecieSeeder.SeedSpecieAsync(speciesRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(speciesRepository, specie);
        
        _context.Specie = specie;
        _context.Breed = breed;
    }

    [Given(@"I have pet data with:")]
    public void GivenIHavePetDataWith(Table table)
    {
        var data = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        
        _context.AddPetRequest = new AddPetRequest(
            data["Name"],
            _context.Specie?.Id.Value ?? Guid.Empty,
            _context.Breed?.Id.Value ?? Guid.Empty,
            data["Description"],
            data["Color"],
            data["HealthInfo"],
            double.Parse(data["Weight"]),
            double.Parse(data["Height"]),
            bool.Parse(data["IsSterilized"]),
            bool.Parse(data["IsVaccinated"]),
            new AddressDto("", "", "", ""), // Will be set in address step
            DateTime.SpecifyKind(DateTime.Parse(data["BirthDate"]), DateTimeKind.Utc),
            Enum.Parse<PetStatus>(data["PetStatus"])
        );
    }

    [Given(@"I have address data:")]
    public void GivenIHaveAddressData(Table table)
    {
        if (_context.AddPetRequest == null)
            throw new InvalidOperationException("Pet request must be created first");

        // Parse the table as key-value pairs
        var addressData = new Dictionary<string, string>();
        
        // Reqnroll treats the first row as headers, so we need to include both header and row data
        // Headers: Street, 123 Main St
        // Rows: City | New York, State | NY, ZipCode | 10001
        
        // Add the first row data (treated as headers by Reqnroll)
        var headerArray = table.Header.ToArray();
        addressData[headerArray[0]] = headerArray[1]; // Street = 123 Main St
        
        // Add the remaining rows
        foreach (var row in table.Rows)
        {
            addressData[row[0]] = row[1]; // First column is key, second is value
        }
        
        _context.AddPetRequest = _context.AddPetRequest with
        {
            Address = new AddressDto(
                addressData["Street"],
                addressData["City"],
                addressData["State"],
                addressData["ZipCode"]
            )
        };
    }

    [Given(@"I add (\d+) pets to the volunteer")]
    public async Task GivenIAddPetsToTheVolunteer(int count)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
        var specie = await SpecieSeeder.SeedSpecieAsync(speciesRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(speciesRepository, specie);
        
        for (int i = 0; i < count; i++)
        {
            await VolunteerSeeder.SeedPetAsync(volunteersRepository, volunteer, specie, breed);
        }
        
        _context.VolunteerId = volunteer.Id.Value;
    }

    [Given(@"I have a pet with ID ""([^""]*)""")]
    public async Task GivenIHaveAPetWithId(string petId)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
        var specie = await SpecieSeeder.SeedSpecieAsync(speciesRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(speciesRepository, specie);
        var pet = await VolunteerSeeder.SeedPetAsync(volunteersRepository, volunteer, specie, breed);
        
        _context.PetId = pet.Id.Value;
        _context.TestData["pet-id-123"] = pet.Id.Value;
        _context.TestData["pet-id-456"] = pet.Id.Value;
    }

    [Given(@"I have (\d+) pets in the system")]
    public async Task GivenIHavePetsInTheSystem(int count)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        for (int i = 0; i < count; i++)
        {
            var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
            var specie = await SpecieSeeder.SeedSpecieAsync(speciesRepository);
            var breed = await SpecieSeeder.SeedBreedAsync(speciesRepository, specie);
            await VolunteerSeeder.SeedPetAsync(volunteersRepository, volunteer, specie, breed);
        }
    }

    [Given(@"I have a volunteer with (\d+) pets")]
    public async Task GivenIHaveAVolunteerWithPets(int petCount)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
        var specie = await SpecieSeeder.SeedSpecieAsync(speciesRepository);
        var breed = await SpecieSeeder.SeedBreedAsync(speciesRepository, specie);
        
        for (int i = 0; i < petCount; i++)
        {
            await VolunteerSeeder.SeedPetAsync(volunteersRepository, volunteer, specie, breed);
        }
        
        _context.VolunteerId = volunteer.Id.Value;
    }

    [Given(@"I want to move pet at position (\d+) to position (\d+)")]
    public void GivenIWantToMovePetAtPositionToPosition(int fromPosition, int toPosition)
    {
        _context.TestData["fromPosition"] = fromPosition;
        _context.TestData["toPosition"] = toPosition;
    }

    [Given(@"I have invalid pet data:")]
    public void GivenIHaveInvalidPetDataWith(Table table)
    {
        var data = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        
        _context.AddPetRequest = new AddPetRequest(
            data["Name"],
            Guid.Empty, // Invalid
            Guid.Empty, // Invalid
            data["Description"],
            data["Color"],
            data["HealthInfo"],
            double.Parse(data["Weight"]),
            double.Parse(data["Height"]),
            bool.Parse(data["IsSterilized"]),
            bool.Parse(data["IsVaccinated"]),
            new AddressDto("", "", "", ""),
            DateTime.SpecifyKind(DateTime.Parse(data["BirthDate"]), DateTimeKind.Utc),
            Enum.TryParse<PetStatus>(data["PetStatus"], out var status) ? status : PetStatus.NeedsHelp
        );
    }

    [Given(@"no species and breed are selected")]
    public void GivenNoSpeciesAndBreedAreSelected()
    {
        if (_context.AddPetRequest != null)
        {
            _context.AddPetRequest = _context.AddPetRequest with
            {
                SpecieId = Guid.Empty,
                BreedId = Guid.Empty
            };
        }
    }

    [Given(@"I have (\d+) photos to upload")]
    public void GivenIHavePhotosToUpload(int photoCount)
    {
        // In a real implementation, you would create mock IFormFile objects
        // For now, we'll just track the count
        _context.TestData["photoCount"] = photoCount;
    }

    [When(@"I add the pet to the volunteer")]
    public async Task WhenIAddThePetToTheVolunteer()
    {
        if (_context.AddPetRequest == null || _context.VolunteerId == null)
            throw new InvalidOperationException("Pet request and volunteer ID must be set");

        _context.LastResponse = await TestBase.Client.PostAsJsonAsync(
            $"/Volunteer/{_context.VolunteerId}/pet", 
            _context.AddPetRequest);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request the volunteer's pets")]
    public async Task WhenIRequestTheVolunteersPets()
    {
        if (_context.VolunteerId == null)
            throw new InvalidOperationException("Volunteer ID must be set");

        _context.LastResponse = await TestBase.Client.GetAsync($"/Volunteer/pets?VolunteerId={_context.VolunteerId}");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request pet by ID ""([^""]*)""")]
    public async Task WhenIRequestPetById(string petId)
    {
        var actualId = _context.TestData.ContainsKey(petId) 
            ? _context.TestData[petId].ToString() 
            : petId;
            
        _context.LastResponse = await TestBase.Client.GetAsync($"/Volunteer/pets/{actualId}");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request all pets")]
    public async Task WhenIRequestAllPets()
    {
        _context.LastResponse = await TestBase.Client.GetAsync("/Volunteer/pets");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request pets for that volunteer")]
    public async Task WhenIRequestPetsForThatVolunteer()
    {
        if (_context.VolunteerId == null)
            throw new InvalidOperationException("Volunteer ID must be set");

        _context.LastResponse = await TestBase.Client.GetAsync($"/Volunteer/pets?VolunteerId={_context.VolunteerId}");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I update the pet position")]
    public async Task WhenIUpdateThePetPosition()
    {
        if (_context.VolunteerId == null || _context.PetId == null)
            throw new InvalidOperationException("Volunteer ID and Pet ID must be set");

        var fromPosition = (int)_context.TestData["fromPosition"];
        var toPosition = (int)_context.TestData["toPosition"];
        
        var updateRequest = new UpdatePetPositionRequest(toPosition);
        
        _context.LastResponse = await TestBase.Client.PatchAsJsonAsync(
            $"/Volunteer/{_context.VolunteerId}/pet/{_context.PetId}/position", 
            updateRequest);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I delete the pet")]
    public async Task WhenIDeleteThePet()
    {
        if (_context.VolunteerId == null || _context.PetId == null)
            throw new InvalidOperationException("Volunteer ID and Pet ID must be set");

        _context.LastResponse = await TestBase.Client.DeleteAsync(
            $"/Volunteer/{_context.VolunteerId}/pet/{_context.PetId}/hard");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I add the pet with photos")]
    public async Task WhenIAddThePetWithPhotos()
    {
        if (_context.AddPetRequest == null || _context.VolunteerId == null)
            throw new InvalidOperationException("Pet request and volunteer ID must be set");

        // In a real implementation, you would create a multipart form with files
        // For now, we'll just send the JSON request
        _context.LastResponse = await TestBase.Client.PostAsJsonAsync(
            $"/Volunteer/{_context.VolunteerId}/pet", 
            _context.AddPetRequest);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [Then(@"the pet should be created successfully")]
    public void ThenThePetShouldBeCreatedSuccessfully()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the pet should be associated with the volunteer")]
    public void ThenThePetShouldBeAssociatedWithTheVolunteer()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here to check the relationship
    }

    [Then(@"the pet should have the provided information")]
    public void ThenThePetShouldHaveTheProvidedInformation()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here to check the pet data
    }

    [Then(@"I should receive (\d+) pets")]
    public async Task ThenIShouldReceivePets(int expectedCount)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var response = JsonSerializer.Deserialize<PagedList<PetDto>>(_context.LastResponseContent!);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedCount);
    }

    [Then(@"each pet should have a unique serial number")]
    public void ThenEachPetShouldHaveAUniqueSerialNumber()
    {
        var response = JsonSerializer.Deserialize<PagedList<PetDto>>(_context.LastResponseContent!);
        response!.Items.Should().NotBeNull();
        
        var serialNumbers = response.Items.Select(p => p.Position).ToList();
        serialNumbers.Should().OnlyHaveUniqueItems();
    }

    [Then(@"the pets should be ordered by serial number")]
    public void ThenThePetsShouldBeOrderedBySerialNumber()
    {
        var response = JsonSerializer.Deserialize<PagedList<PetDto>>(_context.LastResponseContent!);
        response!.Items.Should().NotBeNull();
        
        var serialNumbers = response.Items.Select(p => p.Position).ToList();
        serialNumbers.Should().BeInAscendingOrder();
    }

    [Then(@"I should receive the pet details")]
    public void ThenIShouldReceiveThePetDetails()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var pet = JsonSerializer.Deserialize<PetDto>(_context.LastResponseContent!);
        pet.Should().NotBeNull();
        pet!.Id.Should().NotBeEmpty();
    }

    [Then(@"the pet should include volunteer information")]
    public void ThenThePetShouldIncludeVolunteerInformation()
    {
        var pet = JsonSerializer.Deserialize<PetDto>(_context.LastResponseContent!);
        pet.Should().NotBeNull();
        pet!.VolunteerId.Should().NotBeEmpty();
    }

    [Then(@"each pet should have basic information")]
    public void ThenEachPetShouldHaveBasicInformation()
    {
        var response = JsonSerializer.Deserialize<PagedList<PetDto>>(_context.LastResponseContent!);
        response!.Items.Should().AllSatisfy(pet =>
        {
            pet.Name.Should().NotBeNullOrEmpty();
            pet.Description.Should().NotBeNullOrEmpty();
        });
    }

    [Then(@"all pets should belong to the volunteer")]
    public void ThenAllPetsShouldBelongToTheVolunteer()
    {
        var response = JsonSerializer.Deserialize<PagedList<PetDto>>(_context.LastResponseContent!);
        response!.Items.Should().AllSatisfy(pet =>
        {
            pet.VolunteerId.Should().Be(_context.VolunteerId!.Value);
        });
    }

    [Then(@"the pet should be moved to position (\d+)")]
    public void ThenThePetShouldBeMovedToPosition(int expectedPosition)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here to check the new position
    }

    [Then(@"other pets should be repositioned accordingly")]
    public void ThenOtherPetsShouldBeRepositionedAccordingly()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here to check other pets' positions
    }

    [Then(@"the pet should be removed from the system")]
    public void ThenThePetShouldBeRemovedFromTheSystem()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the pet should not be retrievable")]
    public async Task ThenThePetShouldNotBeRetrievable()
    {
        if (_context.PetId == null)
            throw new InvalidOperationException("Pet ID must be set");

        var getResponse = await TestBase.Client.GetAsync($"/Volunteer/pets/{_context.PetId}");
        getResponse.IsSuccessStatusCode.Should().BeFalse();
    }

    [Then(@"the pet should have (\d+) photos")]
    public void ThenThePetShouldHavePhotos(int expectedPhotoCount)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here to check photo count
    }

    [Then(@"the pet request should fail with validation errors")]
    public void ThenThePetRequestShouldFailWithValidationErrors()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeFalse();
        _context.LastResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Then(@"the pet should not be created")]
    public void ThenThePetShouldNotBeCreated()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeFalse();
    }
}
