using PetFamily.Application.Features.Volunteers.DTO;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;

public record UpdateVolunteerSocialNetworksRequest(
    Guid VolunteerId,
    UpdateVolunteerSocialNetworksDto Dto
    );

public record UpdateVolunteerSocialNetworksDto(
    IEnumerable<SocialNetworkDto> SocialNetworks 
);
    
