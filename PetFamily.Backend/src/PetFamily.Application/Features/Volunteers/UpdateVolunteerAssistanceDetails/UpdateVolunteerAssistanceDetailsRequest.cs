using PetFamily.Application.Features.Volunteers.DTO;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;

public record UpdateVolunteerAssistanceDetailsRequest(
    Guid VolunteerId,
    UpdateVolunteerAssistanceDetailsDto Dto
    );

public record UpdateVolunteerAssistanceDetailsDto(
    IEnumerable<AssistanceDetailsDto> AssistanceDetails 
);
    