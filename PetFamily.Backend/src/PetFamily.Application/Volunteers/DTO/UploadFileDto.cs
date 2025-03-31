namespace PetFamily.Application.Volunteers.DTO;

public record UploadFileDto
{
    public Stream Content { get; }
    public string FileName { get; }

    public UploadFileDto(Stream content, string fileName)
    {
        Content = content;
        FileName = fileName;
    }
}