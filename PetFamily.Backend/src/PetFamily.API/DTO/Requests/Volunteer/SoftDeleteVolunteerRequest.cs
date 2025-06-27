using PetFamily.Application.Features.Volunteers.Commands.SoftDelete;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record SoftDeleteVolunteerRequest;

public static class SoftDeleteVolunteerRequestExtensions
{
    public static SoftDeleteVolunteerCommand ToCommand(
        this SoftDeleteVolunteerRequest request, Guid volunteerId)
        => new(volunteerId);
}