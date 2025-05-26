using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerSocialNetworks;

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