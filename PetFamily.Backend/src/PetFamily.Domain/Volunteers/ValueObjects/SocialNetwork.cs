using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }
    
    public string Name { get; }
    public string Url { get; }
    
    public static Result<SocialNetwork, Error> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Social network name");
        if (string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsRequired("Social network URL");
        
        var socialNetwork = new SocialNetwork(name, url);
        
        return socialNetwork;
    }
}