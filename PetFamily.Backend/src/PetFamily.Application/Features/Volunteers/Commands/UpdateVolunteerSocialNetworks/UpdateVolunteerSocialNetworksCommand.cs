using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks
{
    public record UpdateVolunteerSocialNetworksCommand(
        Guid VolunteerId,
        IEnumerable<SocialNetworkDto> SocialNetworks
    ) : ICommand<Guid>;
}