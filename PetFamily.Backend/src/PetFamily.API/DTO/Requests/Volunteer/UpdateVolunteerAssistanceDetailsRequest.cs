using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;

namespace PetFamily.API.DTO.Requests.Volunteer;

public sealed record UpdateVolunteerAssistanceDetailsRequest(
    IEnumerable<AssistanceDetailsDto> AssistanceDetail);

public static class UpdateVolunteerAssistanceDetailsRequestExtensions
{
    public static UpdateVolunteerAssistanceDetailsCommand ToCommand(
        this UpdateVolunteerAssistanceDetailsRequest request,
        Guid id)
        => new UpdateVolunteerAssistanceDetailsCommand(id, request.AssistanceDetail);
}
