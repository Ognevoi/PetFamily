using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Photo
{
    private const int MAX_FILE_NAME_LENGTH = 200;

    private static readonly string[] ALLOWED_EXTENSIONS = [".jpg", ".jpeg", ".png"];

    [JsonConstructor]
    private Photo(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; }

    public static Result<Photo, Error> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsRequired("Photo");

        if (fileName.Length > MAX_FILE_NAME_LENGTH)
            return Errors.General.ValueIsInvalid("Photo");

        var fileExtension = Path.GetExtension(fileName);
        if (!ALLOWED_EXTENSIONS.Contains(fileExtension))
            return Errors.General.ValueIsInvalid("Photo");

        return new Photo(fileName);
    }
}