namespace PetFamily.Application.Volunteers.DTO;

public record SocialNetworkDto
{
    public string Name { get; }
    public string Url { get; }
    
    public SocialNetworkDto(string name, string url)
    {
        Name = name;
        Url = url;
    }
}


