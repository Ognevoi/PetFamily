using PetFamily.Application.Features.Volunteers.Queries.GetVolunteers;

namespace PetFamily.API.DTO.Requests.Volunteer;

public record GetVolunteerWithPaginationRequest(int Page = 1, int Size = 10, Guid? VolunteerId = null, string? Name = null)
{
    public GetVolunteerWithPaginationQuery ToQuery() => new(
        Page,
        Size,
        VolunteerId,
        Name
    );
}