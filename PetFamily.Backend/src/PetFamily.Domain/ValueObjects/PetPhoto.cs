using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record PetPhoto
{
    private PetPhoto(string url)
    {
        Url = url;
    }
    
    public string Url { get; }
    public string FileName { get; }
    
    public static Result<PetPhoto> Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "Photo URL is required";
        
        string fileName = Path.GetFileName(new Uri(url).AbsolutePath);

        if (string.IsNullOrWhiteSpace(fileName))
            return "Photo file name could not be determined from the URL";

        return new PetPhoto(url);
    }
}