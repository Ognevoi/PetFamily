using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.UpdateVolunteerAssistanceDetails;

public record UpdateVolunteerAssistanceDetailsRequest(
    Guid VolunteerId,
    UpdateVolunteerAssistanceDetailsDto Dto
    );

public record UpdateVolunteerAssistanceDetailsDto(
    IEnumerable<AssistanceDetailsDto> AssistanceDetails 
);
    