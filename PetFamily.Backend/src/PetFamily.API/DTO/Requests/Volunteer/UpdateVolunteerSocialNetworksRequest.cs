using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerSocialNetworks;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record UpdateVolunteerSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks);

public static class UpdateVolunteerSocialNetworksRequestExtensions
{
    public static UpdateVolunteerSocialNetworksCommand ToCommand(
        this UpdateVolunteerSocialNetworksRequest request,
        Guid id)
        => new UpdateVolunteerSocialNetworksCommand(id, request.SocialNetworks);
}