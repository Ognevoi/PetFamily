using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;

public record GetVolunteerWithPaginationQuery(int Page, int Size,
    Guid? VolunteerId, int? Age, string? Name) : IQuery;