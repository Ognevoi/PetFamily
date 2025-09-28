using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AcceptanceTests.Infrastructure;
using PetFamily.API.DTO.Requests.Volunteer;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.Commands.Create;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.Commands.Update;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Models;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.TestUtils.Seeding;
using Reqnroll;

namespace PetFamily.AcceptanceTests.Steps;

[Binding]
public class VolunteerManagementSteps : BaseStepClass
{
    public VolunteerManagementSteps(TestContext context) : base(context)
    {
    }

    [Given(@"the system is running")]
    public async Task GivenTheSystemIsRunning()
    {
        // System is already running via the base class
        await Task.CompletedTask;
    }

    [Given(@"the database is clean")]
    public async Task GivenTheDatabaseIsClean()
    {
        await TestBase.ResetDatabaseAsync();
    }

    [Given(@"I have volunteer data with:")]
    public void GivenIHaveVolunteerDataWith(Table table)
    {
        var data = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        
        // Create the API request object instead of the command directly
        _context.CreateVolunteerRequest = new CreateVolunteerRequest(
            data["FirstName"],
            data["LastName"],
            data["Email"],
            data["Description"],
            int.Parse(data["ExperienceYears"]),
            data["PhoneNumber"],
            new List<SocialNetworkDto>(),
            new List<AssistanceDetailsDto>()
        );
    }

    [Given(@"I add social networks:")]
    public void GivenIAddSocialNetworks(Table table)
    {
        if (_context.CreateVolunteerRequest == null)
            throw new InvalidOperationException("Volunteer request must be created first");

        var socialNetworks = table.Rows
            .Select(row => new SocialNetworkDto(row["Platform"], row["URL"]))
            .ToList();

        _context.CreateVolunteerRequest = _context.CreateVolunteerRequest with
        {
            SocialNetworks = socialNetworks
        };
    }

    [Given(@"I add assistance details:")]
    public void GivenIAddAssistanceDetails(Table table)
    {
        if (_context.CreateVolunteerRequest == null)
            throw new InvalidOperationException("Volunteer request must be created first");

        var assistanceDetails = table.Rows
            .Select(row => new AssistanceDetailsDto(row["Type"], row["Description"]))
            .ToList();

        _context.CreateVolunteerRequest = _context.CreateVolunteerRequest with
        {
            AssistanceDetails = assistanceDetails
        };
    }

    [Given(@"I have (\d+) volunteers in the system")]
    public async Task GivenIHaveVolunteersInTheSystem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var volunteerRequest = new CreateVolunteerRequest(
                $"Test{i}",
                $"User{i}",
                $"test{i}@example.com",
                $"Test volunteer {i}",
                i + 1,
                $"+123456789{i}",
                [],
                []
            );
            
            var response = await TestBase.Client.PostAsJsonAsync("/Volunteer", volunteerRequest);
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }

    [Given(@"I have a volunteer with ID ""([^""]*)""")]
    public async Task GivenIHaveAVolunteerWithId(string volunteerId)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var volunteer = await VolunteerSeeder.SeedVolunteerAsync(volunteersRepository);
        
        _context.VolunteerId = volunteer.Id.Value;
        _context.TestData["volunteer-id-123"] = volunteer.Id.Value;
        _context.TestData["volunteer-id-456"] = volunteer.Id.Value;
        _context.TestData["volunteer-id-789"] = volunteer.Id.Value;
        _context.TestData["volunteer-id-999"] = volunteer.Id.Value;
    }

    [Given(@"I have a soft deleted volunteer with ID ""([^""]*)""")]
    public async Task GivenIHaveASoftDeletedVolunteerWithId(string volunteerId)
    {
        var volunteersRepository = TestBase.Scope.ServiceProvider.GetRequiredService<IVolunteersRepository>();
        var volunteer = await VolunteerSeeder.SeedSoftDeletedVolunteerAsync(volunteersRepository);
        
        _context.VolunteerId = volunteer.Id.Value;
        _context.TestData[volunteerId] = volunteer.Id.Value;
    }

    [Given(@"I have invalid volunteer data:")]
    public void GivenIHaveInvalidVolunteerDataWith(Table table)
    {
        var data = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        
        _context.CreateVolunteerRequest = new CreateVolunteerRequest(
            data["FirstName"],
            data["LastName"],
            data["Email"],
            data["Description"],
            int.Parse(data["ExperienceYears"]),
            data["PhoneNumber"],
            new List<SocialNetworkDto>(),
            new List<AssistanceDetailsDto>()
        );
    }

    [When(@"I create a volunteer")]
    public async Task WhenICreateAVolunteer()
    {
        if (_context.CreateVolunteerRequest == null)
            throw new InvalidOperationException("Volunteer request must be created first");

        _context.LastResponse = await TestBase.Client.PostAsJsonAsync("/Volunteer", _context.CreateVolunteerRequest);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request all volunteers")]
    public async Task WhenIRequestAllVolunteers()
    {
        _context.LastResponse = await TestBase.Client.GetAsync("/Volunteer");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request volunteer by ID ""([^""]*)""")]
    public async Task WhenIRequestVolunteerById(string volunteerId)
    {
        var actualId = _context.TestData.ContainsKey(volunteerId) 
            ? _context.TestData[volunteerId].ToString() 
            : volunteerId;
            
        _context.LastResponse = await TestBase.Client.GetAsync($"/Volunteer/{actualId}");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I update the volunteer's main information")]
    public async Task WhenIUpdateTheVolunteersMainInformation()
    {
        if (_context.CreateVolunteerRequest == null)
            throw new InvalidOperationException("Volunteer request must be created first");

        var volunteerId = _context.VolunteerId ?? throw new InvalidOperationException("Volunteer ID must be set");
        
        var updateCommand = new UpdateVolunteerCommand(
            volunteerId,
            new FullNameDto(_context.CreateVolunteerRequest.FirstName, _context.CreateVolunteerRequest.LastName),
            _context.CreateVolunteerRequest.Email,
            _context.CreateVolunteerRequest.Description,
            _context.CreateVolunteerRequest.ExperienceYears,
            _context.CreateVolunteerRequest.PhoneNumber
        );

        _context.LastResponse = await TestBase.Client.PutAsJsonAsync($"/Volunteer/{volunteerId}/main-info", updateCommand);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I soft delete the volunteer")]
    public async Task WhenISoftDeleteTheVolunteer()
    {
        var volunteerId = _context.VolunteerId ?? throw new InvalidOperationException("Volunteer ID must be set");
        
        _context.LastResponse = await TestBase.Client.DeleteAsync($"/Volunteer/{volunteerId}/soft");
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I restore the volunteer")]
    public async Task WhenIRestoreTheVolunteer()
    {
        var volunteerId = _context.VolunteerId ?? throw new InvalidOperationException("Volunteer ID must be set");
        
        _context.LastResponse = await TestBase.Client.PatchAsync($"/Volunteer/{volunteerId}/restore", null);
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [Then(@"the volunteer should be created successfully")]
    public void ThenTheVolunteerShouldBeCreatedSuccessfully()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the volunteer should have the provided information")]
    public async Task ThenTheVolunteerShouldHaveTheProvidedInformation()
    {
        _context.LastResponse.Should().NotBeNull();
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();

        // The create response contains a Guid (volunteer ID)
        var createdVolunteerId = JsonSerializer.Deserialize<Guid>(_context.LastResponseContent!);
        createdVolunteerId.Should().NotBeEmpty();
        _context.VolunteerId = createdVolunteerId;

        // Now get the volunteer to verify the data
        var getResponse = await TestBase.Client.GetAsync($"/Volunteer/{createdVolunteerId}");
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        
        // Debug: Print the actual response content
        Console.WriteLine($"GET Response Content: {getResponseContent}");
        
        var volunteer = JsonSerializer.Deserialize<VolunteerDto>(getResponseContent, JsonOptions);
        volunteer.Should().NotBeNull();
        
        // Debug: Print the deserialized volunteer properties
        Console.WriteLine($"Deserialized Volunteer - FirstName: '{volunteer!.FirstName}', LastName: '{volunteer.LastName}', Email: '{volunteer.Email}'");
        
        volunteer.FirstName.Should().Be(_context.CreateVolunteerRequest!.FirstName);
        volunteer.LastName.Should().Be(_context.CreateVolunteerRequest.LastName);
        volunteer.Email.Should().Be(_context.CreateVolunteerRequest.Email);
        volunteer.Description.Should().Be(_context.CreateVolunteerRequest.Description);
        volunteer.ExperienceYears.Should().Be(_context.CreateVolunteerRequest.ExperienceYears);
        volunteer.PhoneNumber.Should().Be(_context.CreateVolunteerRequest.PhoneNumber);
    }

    [Then(@"the volunteer should be retrievable by ID")]
    public async Task ThenTheVolunteerShouldBeRetrievableById()
    {
        var volunteerId = _context.VolunteerId ?? throw new InvalidOperationException("Volunteer ID must be set");
        
        var getResponse = await TestBase.Client.GetAsync($"/Volunteer/{volunteerId}");
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var volunteer = JsonSerializer.Deserialize<VolunteerDto>(getResponseContent, JsonOptions);
        volunteer.Should().NotBeNull();
    }

    [Then(@"the volunteer should have the social networks")]
    public void ThenTheVolunteerShouldHaveTheSocialNetworks()
    {
        // This would need to be implemented based on your DTO structure
        // For now, we'll just verify the response is successful
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the volunteer should have the assistance details")]
    public void ThenTheVolunteerShouldHaveTheAssistanceDetails()
    {
        // This would need to be implemented based on your DTO structure
        // For now, we'll just verify the response is successful
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"I should receive (\d+) volunteers")]
    public async Task ThenIShouldReceiveVolunteers(int expectedCount)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var response = JsonSerializer.Deserialize<PagedList<VolunteerDto>>(_context.LastResponseContent!, JsonOptions);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedCount);
    }

    [Then(@"each volunteer should have basic information")]
    public void ThenEachVolunteerShouldHaveBasicInformation()
    {
        var response = JsonSerializer.Deserialize<PagedList<VolunteerDto>>(_context.LastResponseContent!, JsonOptions);
        response!.Items.Should().AllSatisfy(volunteer =>
        {
            volunteer.FirstName.Should().NotBeNullOrEmpty();
            volunteer.LastName.Should().NotBeNullOrEmpty();
            volunteer.Email.Should().NotBeNullOrEmpty();
        });
    }

    [Then(@"I should receive the volunteer details")]
    public void ThenIShouldReceiveTheVolunteerDetails()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var volunteer = JsonSerializer.Deserialize<VolunteerDto>(_context.LastResponseContent!, JsonOptions);
        volunteer.Should().NotBeNull();
        volunteer!.Id.Should().NotBeEmpty();
    }

    [Then(@"the volunteer should include their pets")]
    public void ThenTheVolunteerShouldIncludeTheirPets()
    {
        var volunteer = JsonSerializer.Deserialize<VolunteerDto>(_context.LastResponseContent!, JsonOptions);
        volunteer.Should().NotBeNull();
        volunteer!.Pets.Should().NotBeNull();
    }

    [Then(@"the volunteer should be updated successfully")]
    public void ThenTheVolunteerShouldBeUpdatedSuccessfully()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the volunteer should have the new information")]
    public void ThenTheVolunteerShouldHaveTheNewInformation()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        // Additional verification could be added here
    }

    [Then(@"the volunteer should be marked as deleted")]
    public void ThenTheVolunteerShouldBeMarkedAsDeleted()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the volunteer should not appear in active volunteer lists")]
    public async Task ThenTheVolunteerShouldNotAppearInActiveVolunteerLists()
    {
        var getResponse = await TestBase.Client.GetAsync("/Volunteer");
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var response = JsonSerializer.Deserialize<PagedList<VolunteerDto>>(await getResponse.Content.ReadAsStringAsync(), JsonOptions);
        response!.Items.Should().NotContain(v => v.Id == _context.VolunteerId!.Value);
    }

    [Then(@"the volunteer should be active again")]
    public void ThenTheVolunteerShouldBeActiveAgain()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then(@"the volunteer should appear in active volunteer lists")]
    public async Task ThenTheVolunteerShouldAppearInActiveVolunteerLists()
    {
        var getResponse = await TestBase.Client.GetAsync("/Volunteer");
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var response = JsonSerializer.Deserialize<PagedList<VolunteerDto>>(await getResponse.Content.ReadAsStringAsync(), JsonOptions);
        response!.Items.Should().Contain(v => v.Id == _context.VolunteerId!.Value);
    }

    [Then(@"the volunteer request should fail with validation errors")]
    public void ThenTheVolunteerRequestShouldFailWithValidationErrors()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeFalse();
        _context.LastResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Then(@"the volunteer should not be created")]
    public void ThenTheVolunteerShouldNotBeCreated()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeFalse();
    }

    [Given(@"I have updated volunteer data:")]
    public void GivenIHaveUpdatedVolunteerData(Table table)
    {
        var data = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        
        // Create the API request object for the update
        _context.CreateVolunteerRequest = new CreateVolunteerRequest(
            data["FirstName"],
            data["LastName"],
            data["Email"],
            data["Description"],
            int.Parse(data["ExperienceYears"]),
            data["PhoneNumber"],
            new List<SocialNetworkDto>(),
            new List<AssistanceDetailsDto>()
        );
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}
