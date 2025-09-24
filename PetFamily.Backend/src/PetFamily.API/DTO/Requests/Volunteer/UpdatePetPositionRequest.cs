using PetFamily.Application.Features.Volunteers.Commands.UpdatePetPosition;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record UpdatePetPositionRequest(
    int NewPosition);

public static class UpdatePetPositionRequestExtensions
{
    public static UpdatePetPositionCommand ToCommand(
        this UpdatePetPositionRequest request,
        Guid volunteerId,
        Guid petId)
        => new UpdatePetPositionCommand(volunteerId, petId, request.NewPosition);
}