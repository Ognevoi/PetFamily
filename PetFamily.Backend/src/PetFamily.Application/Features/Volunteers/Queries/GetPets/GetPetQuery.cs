using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Models;


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
) : IQuery<PagedList<PetDto>>;