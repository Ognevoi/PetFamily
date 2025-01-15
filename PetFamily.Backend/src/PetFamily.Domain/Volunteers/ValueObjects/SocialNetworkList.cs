using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record SocialNetworkList
{
    private static readonly List<SocialNetwork> _socialNetworks = new();

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public static Result AddSocialNetwork(SocialNetwork socialNetwork)
    {
        if (socialNetwork == null)
            return "Social network is required";

        _socialNetworks.Add(socialNetwork);

        return Result.Success();
    }
}