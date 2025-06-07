using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Queries.GetPets;

public record GetPetWithPaginationQuery(
    int Page,
    int Size,
    Guid? VolunteerId,
    Guid? BreedId,
    Guid? SpecieId,
    string? Name,
    int? Age,
    string? SortBy,
    string? SortDirection
) : IQuery;