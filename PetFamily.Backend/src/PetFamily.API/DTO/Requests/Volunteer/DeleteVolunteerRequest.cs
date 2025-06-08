using PetFamily.Application.Features.Volunteers.Commands.HardDelete;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record DeleteVolunteerRequest;

public static class DeleteVolunteerRequestExtensions
{
    public static DeleteVolunteerCommand ToCommand(
        this DeleteVolunteerRequest request, Guid volunteerId)
        => new(volunteerId);
}