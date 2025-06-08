using PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetVolunteerWithPaginationRequest(int Page, int Size, Guid? VolunteerId, int? Age, string? Name)
{
    public GetVolunteerWithPaginationQuery ToQuery() => new(
        Page,
        Size,
        VolunteerId,
        Age,
        Name
    );
}
