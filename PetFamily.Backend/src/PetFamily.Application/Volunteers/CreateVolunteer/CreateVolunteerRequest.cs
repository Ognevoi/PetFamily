using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.API.Controllers;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails
);