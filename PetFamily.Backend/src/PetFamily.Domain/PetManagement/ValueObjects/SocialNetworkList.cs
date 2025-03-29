using System.Text.Json.Serialization;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class SocialNetworkList
{
    public List<SocialNetwork> SocialNetworks { get; private set; }
    
    private SocialNetworkList()
    {
        SocialNetworks = new List<SocialNetwork>();
    }

    [JsonConstructor]
    public SocialNetworkList(List<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks ?? new List<SocialNetwork>();
    }
    
}