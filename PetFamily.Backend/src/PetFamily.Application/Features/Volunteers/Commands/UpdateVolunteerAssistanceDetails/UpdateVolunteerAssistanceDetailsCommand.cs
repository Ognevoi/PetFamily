using PetFamily.Application.Features.Volunteers.Commands.DTO;
using PetFamily.Application.Interfaces;

namespace PetFamily.Application.Features.Volunteers.Commands.UpdateVolunteerAssistanceDetails;

public sealed record UpdateVolunteerAssistanceDetailsCommand(
    Guid VolunteerId,
    IEnumerable<AssistanceDetailsDto> AssistanceDetails)
    : ICommand;