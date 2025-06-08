namespace PetFamily.Application.Features.Volunteers.Commands.DTO;

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