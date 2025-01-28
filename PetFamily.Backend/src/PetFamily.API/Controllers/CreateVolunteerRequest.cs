using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.API.Controllers;

public record CreateVolunteerRequest(
    string FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails
    );