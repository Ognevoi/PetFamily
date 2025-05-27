using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;

public record UpdateVolunteerSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworkDto> SocialNetworks
) : ICommand;