using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record PetPhoto
{
    private PetPhoto(string url,  string fileName)
    {
        Url = url;
        FileName = fileName;
    }

    public string Url { get; }
    public string FileName { get; }
    
    public static Result<PetPhoto, Error> Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsRequired("Photo URL");
        
        string fileName = Path.GetFileName(new Uri(url).AbsolutePath);

        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsRequired("Photo File Name");

        return new PetPhoto(url, fileName);
    }
}