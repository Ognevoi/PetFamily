using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }
    
    public string Name { get; }
    public string Url { get; }
    
    public static Result<SocialNetwork> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<SocialNetwork>("Social network name should not be empty");
        
        if (string.IsNullOrWhiteSpace(url))
            return Result.Failure<SocialNetwork>("Social network URL should not be empty");
        
        var socialNetwork = new SocialNetwork(name, url);
        
        return Result.Success(socialNetwork);
    }
}