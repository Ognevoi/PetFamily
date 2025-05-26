using PetFamily.Application.Features.Volunteers.Restore;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record RestoreVolunteerRequest;

public static class RestoreVolunteerCommandExtensions
{
    public static RestoreVolunteerCommand ToCommand(
        this RestoreVolunteerRequest request, Guid volunteerId)
        => new RestoreVolunteerCommand(volunteerId);
}