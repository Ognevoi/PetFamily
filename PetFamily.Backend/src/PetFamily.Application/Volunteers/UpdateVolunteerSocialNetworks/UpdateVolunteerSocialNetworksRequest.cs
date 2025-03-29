using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UpdateVolunteerSocialNetworks;

public record UpdateVolunteerSocialNetworksRequest(
    Guid VolunteerId,
    UpdateVolunteerSocialNetworksDto Dto
    );

public record UpdateVolunteerSocialNetworksDto(
    IEnumerable<SocialNetworkDto> SocialNetworks 
);
    
