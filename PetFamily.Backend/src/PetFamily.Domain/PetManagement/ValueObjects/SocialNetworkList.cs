namespace PetFamily.Domain.PetManagement.ValueObjects;

public record SocialNetworkList
{
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    
    // required by EF Core
    private SocialNetworkList()
    {
        SocialNetworks = new List<SocialNetwork>();
    }
    
    public SocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }
    
}