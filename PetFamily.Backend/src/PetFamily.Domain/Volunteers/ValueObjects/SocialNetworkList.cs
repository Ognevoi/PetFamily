namespace PetFamily.Domain.Volunteers.ValueObjects;

public record SocialNetworkList
{
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    
    public SocialNetworkList(List<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }
    
}