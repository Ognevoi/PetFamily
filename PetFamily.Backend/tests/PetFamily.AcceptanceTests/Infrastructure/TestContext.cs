using PetFamily.API.DTO.Requests.Volunteer;
using PetFamily.Application.Features.Volunteers.Commands.Create;
using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;

namespace PetFamily.AcceptanceTests.Infrastructure;

public class TestContext
{
    // Test infrastructure
    public AcceptanceTestBase? TestBase { get; set; }

    // Volunteer related
    public CreateVolunteerCommand? CreateVolunteerCommand { get; set; }
    public CreateVolunteerRequest? CreateVolunteerRequest { get; set; }
    public VolunteerDto? CreatedVolunteer { get; set; }
    public List<VolunteerDto>? Volunteers { get; set; }
    public Guid? VolunteerId { get; set; }

    // Pet related
    public AddPetRequest? AddPetRequest { get; set; }
    public PetDto? CreatedPet { get; set; }
    public List<PetDto>? Pets { get; set; }
    public Guid? PetId { get; set; }

    // Species related
    public Specie? Specie { get; set; }
    public Breed? Breed { get; set; }
    public List<SpecieDto>? Species { get; set; }

    // API Response
    public HttpResponseMessage? LastResponse { get; set; }
    public string? LastResponseContent { get; set; }

    // Test data
    public Dictionary<string, object> TestData { get; } = new();

    public void Clear()
    {
        CreateVolunteerCommand = null;
        CreateVolunteerRequest = null;
        CreatedVolunteer = null;
        Volunteers = null;
        VolunteerId = null;
        AddPetRequest = null;
        CreatedPet = null;
        Pets = null;
        PetId = null;
        Specie = null;
        Breed = null;
        Species = null;
        LastResponse = null;
        LastResponseContent = null;
        TestData.Clear();
    }
}
