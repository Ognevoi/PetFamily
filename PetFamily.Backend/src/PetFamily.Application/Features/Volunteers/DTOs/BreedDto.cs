using System.Text.Json.Serialization;

namespace PetFamily.Application.Features.Volunteers.DTOs;

public class BreedDto
{
    public Guid Id { get; init; }
    public Guid SpecieId { get; init; }
    public string Name { get; init; } = string.Empty;
    [JsonIgnore] public SpecieDto Specie { get; init; } = null!;

}