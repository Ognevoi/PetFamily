using System.Text.Json.Serialization;

namespace PetFamily.Application.Features.Volunteers.DTOs;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    [JsonIgnore] public VolunteerDto Volunteer { get; init; } = null!;
    public Guid SpecieId { get; init; }
    public Guid BreedId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime BirthDate { get; init; }
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<PhotoDto> Photos { get; init; } = new List<PhotoDto>();
}

public class PhotoDto
{
    public string FileName { get; set; } = string.Empty;
}