namespace PetFamily.Application.Features.Volunteers.DTOs;

public class SpecieDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<BreedDto> Breeds { get; init; } = new();
}