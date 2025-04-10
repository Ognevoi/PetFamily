using PetFamily.Application.Features.Volunteers.DTO;

namespace PetFamily.Application.Features.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int ExperienceYears,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails
);