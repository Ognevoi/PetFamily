using PetFamily.Application.Features.Volunteers.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteerAssistanceDetails;

public sealed record UpdateVolunteerAssistanceDetailsCommand(
    Guid VolunteerId,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails)
    : ICommand;