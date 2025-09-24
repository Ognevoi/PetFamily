using PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetVolunteerWithPaginationRequest(int Page, int Size, Guid? VolunteerId, string? Name)
{
    public GetVolunteerWithPaginationQuery ToQuery() => new(
        Page,
        Size,
        VolunteerId,
        Name
    );
}