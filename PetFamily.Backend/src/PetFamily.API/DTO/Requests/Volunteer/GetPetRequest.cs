using PetFamily.Application.Features.Volunteers.Queries.GetPets;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetPetWithPaginationRequest(
    int Page,
    int Size,
    Guid? VolunteerId,
    Guid? BreedId,
    Guid? SpeciesId,
    string? Name,
    int? Age,
    string? SortBy = null,
    string? SortDirection = null)
{
    public GetPetWithPaginationQuery ToQuery() =>
        new(Page, Size, VolunteerId, BreedId, SpeciesId, Name, Age, SortBy, SortDirection);
}